using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using MagmaLabs.Editor;
using System.Threading;
using MagmaLabs.Utilities.Primitives;
using MagmaLabs.Economy.Security;
using System;
namespace MagmaLabs.Economy{
    [CreateAssetMenu(menuName = "MagmaLabs/Economy/ProgressionNode")]
    public class ProgressionNode : Savable
    {

        public string id { get; private set; }
        [SerializeField] private NodeStatus status;
        [SerializeField] private bool usePrerequisitesInsteadOfContinuations = false;

        [ShowIf("usePrerequisitesInsteadOfContinuations", false)]
        [SerializeField] private List<ProgressionNode> continuationNodes = new List<ProgressionNode>();

        [ShowIf("usePrerequisitesInsteadOfContinuations", true)]
        [SerializeField] private List<ProgressionNode> prerequisiteNodes = new List<ProgressionNode>();

        [SerializeField] private List<ProgressionNode> mutuallyExclusiveNodes = new List<ProgressionNode>();

        static readonly string[] INVALID_NAMES = {"id=", "status="};

        public ProgressionNode(string serialized)
        {
            LoadFromSerialized(serialized);
        }

        void Awake()
        {
        }
        void Start()
        {
            
            if(status == NodeStatus.Unlocked)
            {
                ContinuationUnlock();
            }
            if(status == NodeStatus.Activated)
            {
                foreach(ProgressionNode cont in continuationNodes)
                {
                    PrerequisiteUnlock();
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

            Enum.TryParse<NodeStatus>(unparsedStatus, out status);

            return localSerialized.Substring(secondLocalSeparator);
            

        }
        public void PrerequisiteUnlock()
        {
            foreach(ProgressionNode prereq in prerequisiteNodes){
                if(prereq.status != NodeStatus.Activated)
                {
                    return;
                    //if a single prerequisite is missing, ProgressionNode cannot unlock
                }
            }
            if(status == NodeStatus.Locked)
            {
                status = NodeStatus.Unlocked;
            }
        }
        public void ContinuationUnlock()
        {
            if(status == NodeStatus.Locked)
            {
                status = NodeStatus.Unlocked;
            }
        }
        public void Lock()
        {
            if(status != NodeStatus.Locked)
            {
                status = NodeStatus.Locked;
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