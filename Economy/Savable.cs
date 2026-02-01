using UnityEngine;
using System.Collections.Generic;
using System;
namespace MagmaLabs.Economy{
[System.Serializable]

[Obsolete("Use Savable instead")]
public class Ownable : Savable
    {
        public Ownable(string serialized) : base(serialized)
        {

        }

        public Ownable(string name, Sprite spr) : base(name, spr)
        {

        }
    }

[System.Serializable]

[CreateAssetMenu(fileName = "Savable", menuName = "Scriptable Objects/Savable")]
public class Savable : ScriptableObject
{
    /// <summary>
    /// Object that can be owned in the game economy. Useful for storing items, collectibles, and other in-game assets.
    /// </summary>
    /// <remarks>
    /// </remarks>

    protected const char FILE_SEPARATOR = '\x1C';
    protected const char GROUP_SEPARATOR = '\x1D';  
    /// <summary>
    /// The record separator character, used for separating categories of data in serialized strings.
    /// </summary>
    protected const char RECORD_SEPARATOR = '\x1E'; 
    /// <summary>
    /// The unit separator character, used for separating individual data items in serialized strings.
    /// </summary>
    protected const char UNIT_SEPARATOR = '\x1F';   
    

    [SerializeField]private Sprite sprite;
    /// <summary>
    /// The list of tags associated with this ownable.
    /// </summary>

    /// Default tags:
    /// <list type="bullet">
    /// <item><term>type</term><description>Indicates the type of ownable (e.g., "claimable").</description></item>
    /// <item><term>rarity</term><description>Indicates the rarity of the ownable (e.g., "common").</description></item>
    /// <item><term>hideInInventory</term><description>Indicates whether the ownable should be hidden in the inventory
    [SerializeField]private List<Tag> tags = new List<Tag>() {new Tag("type", "claimable"), new Tag("rarity", "common"), new Tag("hideInInventory", "true")};
    /// <summary>
    /// Initializes a new <see cref="Ownable"/> instance given a name and sprite.
    /// </summary>
    /// <param name="name">The name of the ownable item.</param>
    /// <param name="spr">The sprite of the ownable item.</param>
    /// 
    public Savable(){}
    public Savable(string name, Sprite spr)
    {
        this.name = name;
        sprite = spr;
    }
    /// <summary>
    /// Initializes a new <see cref="Ownable"/> instance from a serialized string.
    /// </summary>
    /// <param name="serialized">The serialized string representation of the ownable.</param>
    public Savable(string serialized)
    {
        int firstBar = serialized.IndexOf(RECORD_SEPARATOR);
        int secondBar = serialized.IndexOf(RECORD_SEPARATOR, firstBar+1);
        int thirdBar = serialized.IndexOf(RECORD_SEPARATOR, secondBar+1);

        int fourthBar = serialized.IndexOf(RECORD_SEPARATOR, thirdBar+1);
        if(firstBar == -1 || secondBar == -1)
        {
            Debug.LogWarning("Serious data corruption. Using default values");
            this.name = "NewOwnable";
            sprite = SpriteManager.instance.placeholder;
        }
        if(thirdBar == -1)
        {
            Debug.LogWarning("Data incomplete: Using default values");
            this.name = "NewOwnable";
            sprite = SpriteManager.instance.placeholder;
            
        }else if(fourthBar != -1)
        {
            Debug.LogWarning("Data possibly corrupted: Using default values");
            this.name = "NewOwnable";
            sprite = SpriteManager.instance.placeholder;
        }
        else
        {
            this.name = serialized.Substring(0, firstBar);
            string spriteName = serialized.Substring(firstBar+1, secondBar - firstBar - 1);
            sprite = SpriteManager.instance.GetSpriteByName(spriteName);
            
            // Debug.Log("Second Bar +1: " + (secondBar + 1));
            // Debug.Log("Third Bar: " + thirdBar);
            // Debug.Log("Serialized Length: " + serialized.Length);

            string combinedNames = serialized.Substring((secondBar + 1), thirdBar - secondBar - 1);
            string combinedValues = serialized.Substring(thirdBar + 1);

            List<string> tagNames = new List<string>();
            List<string> tagValues = new List<string>();


            while(combinedNames.IndexOf(UNIT_SEPARATOR) != -1 && combinedValues.IndexOf(UNIT_SEPARATOR) != -1){

                int nameIndex = combinedNames.IndexOf(UNIT_SEPARATOR);
                int valueIndex = combinedValues.IndexOf(UNIT_SEPARATOR);

                string nameSegment = combinedNames.Substring(0, nameIndex);
                string valueSegment = combinedValues.Substring(0, valueIndex);

                tagNames.Add(nameSegment);
                tagValues.Add(valueSegment);

                combinedNames = combinedNames.Substring(nameIndex + 1);
                combinedValues = combinedValues.Substring(valueIndex + 1);


            }
            
            tagNames.Add(combinedNames);
            tagValues.Add(combinedValues);


            List<Tag> initialize = new List<Tag>();

            for(int i=0; i<tagNames.Count; i++)
            {
                initialize.Add(new Tag(tagNames[i], tagValues[i]));
            }
            tags = initialize;

        }



    }
    /// <summary>
    /// Loads the ownable's data from a serialized string.
    /// </summary>
    /// <param name="serialized">The serialized representation of the ownable.</param>
    /// <returns>
    /// <c>string</c> the remains after loading the relevant serialized code
    /// </returns>
    virtual public string LoadFromSerialized(string serialized)
    {
        int firstBar = serialized.IndexOf(RECORD_SEPARATOR);
        int secondBar = serialized.IndexOf(RECORD_SEPARATOR, firstBar+1);
        int thirdBar = serialized.IndexOf(RECORD_SEPARATOR, secondBar+1);
        //only the first 3 separators are actually used

        

        if(firstBar == -1 || secondBar == -1)
        {
            Debug.LogWarning("Serious data corruption. Using default values");
            this.name = "NewOwnable";
            sprite = sprite = SpriteManager.instance.placeholder;
        }
        if(thirdBar == -1)
        {
            Debug.LogWarning("Data incomplete: Using default values");
            this.name = "NewOwnable";
            sprite = sprite = SpriteManager.instance.placeholder;
            
        }
        else
        {
            this.name = serialized.Substring(0, firstBar);
            string spriteName = serialized.Substring(firstBar+1, secondBar - firstBar - 1);
            sprite = SpriteManager.instance.GetSpriteByName(spriteName);
            
            // Debug.Log("Second Bar +1: " + (secondBar + 1));
            // Debug.Log("Third Bar: " + thirdBar);
            // Debug.Log("Serialized Length: " + serialized.Length);

            string combinedNames = serialized.Substring((secondBar + 1), thirdBar - secondBar - 1);
            string combinedValues = serialized.Substring(thirdBar + 1);

            List<string> tagNames = new List<string>();
            List<string> tagValues = new List<string>();


            while(combinedNames.IndexOf(UNIT_SEPARATOR) != -1 && combinedValues.IndexOf(UNIT_SEPARATOR) != -1){

                int nameIndex = combinedNames.IndexOf(UNIT_SEPARATOR);
                int valueIndex = combinedValues.IndexOf(UNIT_SEPARATOR);

                string nameSegment = combinedNames.Substring(0, nameIndex);
                string valueSegment = combinedValues.Substring(0, valueIndex);

                tagNames.Add(nameSegment);
                tagValues.Add(valueSegment);

                combinedNames = combinedNames.Substring(nameIndex + 1);
                combinedValues = combinedValues.Substring(valueIndex + 1);


            }
            
            tagNames.Add(combinedNames);
            tagValues.Add(combinedValues);


            List<Tag> initialize = new List<Tag>();

            for(int i=0; i<tagNames.Count; i++)
            {
                initialize.Add(new Tag(tagNames[i], tagValues[i]));
            }
            tags = initialize;

        }

        int fourthBar = serialized.IndexOf(RECORD_SEPARATOR, thirdBar+1);
        if(fourthBar != -1)
            {
                return serialized.Substring(fourthBar);
            }
        return "";


    }

    /// <summary>
    /// Stores the ownable's data into a serialized string.
    /// </summary>
    /// <returns>
    /// <c>string</c> representation of the ownable.
    /// </returns>
    virtual public string Serialize()
    {

        if(sprite == null)
        {
            sprite = SpriteManager.instance.placeholder;
        }

        List<string> tagNames = new List<string>();
        List<string> tagValues = new List<string>();
        foreach(Tag tag in tags)
        {
            tagNames.Add(tag.name);
            tagValues.Add(tag.value);
        }


        string combinedNames = string.Join(UNIT_SEPARATOR.ToString(), tagNames.ToArray() );

        string combinedValues = string.Join(UNIT_SEPARATOR.ToString(), tagValues.ToArray() );



        // Debug.Log("Name: " + this.name);
        // Debug.Log("Sprite: " + sprite.name);
        // Debug.Log("Combined Tag Names: " + combinedNames);
        // Debug.Log("Combined Tag Values: " + combinedValues);
        string combined = 
        this.name + 
        RECORD_SEPARATOR + sprite.name + 
        RECORD_SEPARATOR + combinedNames + 
        RECORD_SEPARATOR + combinedValues;
        Debug.Log("Serialized Ownable: " + ToString());
        return combined;

    }

    /// <summary>
    /// Adds a new tag to the ownable.
    /// </summary>
    /// <param name="label">The label of the tag to add.</param>
    /// <param name="value">The value of the tag to add.</param>
    public void AddTag(string label, string value)
    {
        Tag t = new Tag(label, value);
        tags.Add(t);
    }

    /// <summary>
    /// Finds the value associated with a given tag name.
    /// </summary>
    /// <param name="label">The label of the tag to find.</param>
    /// <returns>
    /// <c>string</c> the value of the tag.
    /// </returns>
    /// 
    /// 
    [Obsolete("Use GetTag instead")]
    public string FindTag(string label)
    {
        foreach(Tag t in tags)
        {
            if (t.name.Equals(label))
            {
                return t.value;
            }
        }
        throw new KeyNotFoundException("Tag not found for key " + label);
    }
    /// <summary>
    /// Removes the value associated with a given tag name.
    /// </summary>
    /// <param name="label">The label of the tag to remove.</param>
    
    public string GetTag(string label)
        {
            return FindTag(label);
        }
        public int GetInt(string label)
        {
            string tag = GetTag(label);
            return int.Parse(tag);
        }

        public float GetFloat(string label)
        {
            string tag = GetTag(label);
            return float.Parse(label);
        }


    public void RemoveTag(string label)
    {
        foreach(Tag t in tags)
        {
            if (t.name.Equals(label))
            {
                tags.Remove(t);
                return;
            }
        }
    }
    /// <summary>
    /// Modifies the value associated with a given tag name.
    /// </summary>
    /// <param name="label">The label of the tag to find.</param>
    /// <param name="value">The value to set the tag to.</param>
    [Obsolete("Use SetTag instead")]
    public void ModifyTagValue(string label, string value)
    {
        foreach(Tag t in tags)
        {
            if (t.name.Equals(label))
            {
                t.value = value;
                return;
            }
            AddTag(label, value);
        }
    }
    public void SetTag(string label, string value)
        {
            ModifyTagValue(label, value);
        }
    /// <summary>
    /// Finds a specific <see cref="Ownable"/> from a list by its name.
    /// </summary>
    /// <param name="list">The list of ownables to search through.</param>
    /// <param name="name">The name of the ownable to find.</param>
    /// <returns>The found ownable, or null if not found.</returns>

    /// 
    public static Ownable FindOwnableByName(Ownable[] list, string name)
    {
        foreach (Ownable o in list)
        {
            if (o.name.Equals(name))
            {
                return o;
            }
        }
        return null;
    }
    /// <summary>
        /// Converts the ownable to a string representation.
        /// This representation replaces record and unit separators with pipe and comma characters respectively for easier readability.
        /// It should not be used for storing data.
        /// </summary>
        /// <returns>
        /// <c>string</c> representation of the ownable.
        /// </returns>
         override
    public string ToString()
        {
            string serialization = Serialize();
            string rsToPipe = serialization.Replace(RECORD_SEPARATOR, '|');
            string usToComma = rsToPipe.Replace(UNIT_SEPARATOR, ',');
            return usToComma;
        }

    virtual public Sprite GetSprite()
    {
        return sprite; 
    }

    virtual public void SetSprite(Sprite spr)
    {
        sprite = spr;
    }

    

}

}
