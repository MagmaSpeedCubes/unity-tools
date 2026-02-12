
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MagmaLabs.Editor;
using MagmaLabs.Utilities.Primitives;
using MagmaLabs.Economy.Security;
using MagmaLabs;

namespace MagmaLabs.Economy{
    [System.Serializable]
    [CreateAssetMenu(menuName = "MagmaLabs/Economy/ProgressionNode")]
    public class ProgressionNode : Savable
    {
        const int DEBUG_INFO_LEVEL = 2;
        public static List<ProgressionNode> allNodes = new List<ProgressionNode>();
        public string id { get; private set; }
        public NodeStatus status;
        [SerializeField] private bool usePrerequisitesInsteadOfContinuations = false;

        [ShowIf("usePrerequisitesInsteadOfContinuations", false)]
        public List<ProgressionNode> continuationNodes  = new List<ProgressionNode>();

        [ShowIf("usePrerequisitesInsteadOfContinuations", true)]
        public List<ProgressionNode> prerequisiteNodes  = new List<ProgressionNode>();

        public List<ProgressionNode> mutuallyExclusiveNodes  = new List<ProgressionNode>();

        static readonly string[] INVALID_NAMES = {"id=", "status="};

        public ProgressionNode(string serialized)
        {
            LoadFromSerialized(serialized);
        }

        void OnEnable()
        {   
            allNodes.Add(this);
            RefreshInstance();
        }

        public static void Refresh()
        {
            foreach(ProgressionNode node in allNodes){
                node.RefreshInstance();
            }
        }
        public void RefreshInstance()
        {
            DebugEnhanced.LogInfoLevel("Refreshing", 1, DEBUG_INFO_LEVEL);
            if (status == NodeStatus.Activated)
            {
                DebugEnhanced.LogInfoLevel("Activated", 2, DEBUG_INFO_LEVEL);
                foreach(ProgressionNode cont in continuationNodes)
                {
                    DebugEnhanced.LogInfoLevel("Trying to unlock continuation", 2, DEBUG_INFO_LEVEL);
                    cont.TryUnlock(this);
                }
                foreach(ProgressionNode mutex in mutuallyExclusiveNodes)
                {
                    mutex.TryLock(this);
                }
            }
        }
        void LoadFromID(string id)
        {
                try
                {
                    string serialized = SecureProfileStats.instance.LoadString(id);
                    LoadFromSerialized(serialized);
                    
                }catch(KeyNotFoundException e)
                {
                    string serialized = Serialize();
                    SecureProfileStats.instance.SaveString(id, serialized);
                }
            
        }
        public void Save(){
            string serialized = Serialize();
            SecureProfileStats.instance.SaveString(id, serialized);
        }
        void LateLoadFromID(string id)
        {
            Thread.Sleep(1);
            LoadFromID(id);
        }
        override public string Serialize()
        {
            foreach(string inv in INVALID_NAMES)
            {
                if (inv.Equals(this.name))
                {
                    throw new System.ArgumentException("Name " + INVALID_NAMES + " is an invalid name and could cause serialization errors");
                }
                if (inv.Equals(id))
                {
                    throw new System.ArgumentException("ID " + INVALID_NAMES + " is an invalid ID and could cause serialization errors");
                }
            }

            string baseData = base.Serialize();

            string nodeData = "";
            nodeData += "id=" + id + UNIT_SEPARATOR;
            nodeData += "status=" + status.ToString();

            string combined = baseData + RECORD_SEPARATOR + nodeData;

            return combined;

        }
        override public string LoadFromSerialized(string serialized)
        {
            string localSerialized = base.LoadFromSerialized(serialized);//the base section contains name, sprite, and tag data
            int firstLocalSeparator = localSerialized.IndexOf(RECORD_SEPARATOR);//the first section contains id and status data
            int secondLocalSeparator = localSerialized.IndexOf(RECORD_SEPARATOR, firstLocalSeparator+1);//the save only uses one separator, this is to check for additional info

            string localSegment = localSerialized.Substring(0, secondLocalSeparator);

            id = Strings.extractBetween(localSegment, "id=", ""+UNIT_SEPARATOR, 0);
            string unparsedStatus = Strings.extractBetween(localSegment, "status=", ""+UNIT_SEPARATOR, 0);
            NodeStatus temp;

            Enum.TryParse<NodeStatus>(unparsedStatus, out temp);
            status = temp;

            return localSerialized.Substring(secondLocalSeparator);
            

        }

        public void Lock()
        {
            status = NodeStatus.Locked;

        } 

        public bool TryUnlock(ProgressionNode source)
        {
            if (source.status == NodeStatus.Activated && status == NodeStatus.Locked)
            {
                if (UsesPrerequisites())
                {
                    foreach(ProgressionNode prereq in prerequisiteNodes)
                    {
                        if(prereq.status != NodeStatus.Activated){return false;}
                    }
                    status = NodeStatus.Unlocked;
                    return true;
                }
                else
                {
                    if (source.continuationNodes.IndexOf(this) != -1)
                    {
                        status = NodeStatus.Unlocked;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TryLock(ProgressionNode source)
        {
            if (source.status == NodeStatus.Activated && source.mutuallyExclusiveNodes.IndexOf(this)!=-1){}
            {
                Lock();
                return true;
            }
            return false;
        }
        public void Activate()
        {
            status = NodeStatus.Activated;
            foreach(ProgressionNode cont in continuationNodes)
            {
                cont.TryUnlock(this);
            }
        }
        public bool UsesPrerequisites()
        {
            return usePrerequisitesInsteadOfContinuations;
        }
        

    }

    [System.Serializable]
    public enum NodeStatus
    {
        Locked,
        Unlocked,
        Activated
    }

}