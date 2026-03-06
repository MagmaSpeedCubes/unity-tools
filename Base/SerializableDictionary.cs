using System.Collections.Generic;
using System.Collections;

using UnityEngine;

namespace MagmaLabs{
[System.Serializable]
public class SerializableKeyValue<T>
{
    public string key;
    public T value;
    
    public SerializableKeyValue() { }
    public SerializableKeyValue(string key, T value)
    {
        this.key = key;
        this.value = value;
    }
}

[System.Serializable]
public class SerializableDictionary<T> : IEnumerable<SerializableKeyValue<T>>
{
    [SerializeField]
    private List<SerializableKeyValue<T>> items = new List<SerializableKeyValue<T>>();

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    public bool TryGetValue(string key, out T value)
    {
        foreach (var item in items)
        {
            if (item.key == key)
            {
                value = item.value;
                return true;
            }
        }
        value = default;
        return false;
    }

    /// <summary>
    /// Gets the value associated with the specified key, or returns default if not found.
    /// </summary>
    public T Get(string key, T defaultValue = default)
    {
        TryGetValue(key, out T value);
        return value != null ? value : defaultValue;
    }

    /// <summary>
    /// Sets or adds a key-value pair.
    /// </summary>
    public void Set(string key, T value)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].key == key)
            {
                items[i].value = value;
                return;
            }
        }
        items.Add(new SerializableKeyValue<T>(key, value));
    }

    /// <summary>
    /// Removes the entry with the specified key.
    /// </summary>
    public bool Remove(string key)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].key == key)
            {
                items.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the dictionary contains the specified key.
    /// </summary>
    public bool ContainsKey(string key)
    {
        foreach (var item in items)
        {
            if (item.key == key)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Clears all entries from the dictionary.
    /// </summary>
    public void Clear()
    {
        items.Clear();
    }

    /// <summary>
    /// Gets the number of items in the dictionary.
    /// </summary>
    public int Count => items.Count;

    /// <summary>
    /// Gets the list of all items.
    /// </summary>
    public List<SerializableKeyValue<T>> Items => items;

    /// <summary>
    /// Indexer for getting/setting values.
    /// </summary>
    public T this[string key]
    {
        get => Get(key);
        set => Set(key, value);
    }

    public System.Collections.Generic.IEnumerator<SerializableKeyValue<T>> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return items.GetEnumerator();
    }

    /// <summary>
    /// Converts the dictionary to a string representation.
    /// Format: "key1=value1;key2=value2;" where values are converted using ToString().
    /// </summary>
    public string ToString()
    {
        if (items.Count == 0)
            return string.Empty;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var item in items)
        {
            if (sb.Length > 0)
                sb.Append(";");
            sb.Append(item.key).Append("=").Append(item.value?.ToString() ?? string.Empty);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Creates a dictionary from a string representation.
    /// Expected format: "key1=value1;key2=value2;"
    /// Note: This method requires T to be parseable from string. For complex types, consider using JSON serialization instead.
    /// </summary>
    public static SerializableDictionary<T> FromString(string data)
    {
        var dict = new SerializableDictionary<T>();
        if (string.IsNullOrEmpty(data))
            return dict;

        string[] pairs = data.Split(';');
        foreach (string pair in pairs)
        {
            if (string.IsNullOrWhiteSpace(pair))
                continue;

            string[] keyValue = pair.Split('=');
            if (keyValue.Length == 2)
            {
                string key = keyValue[0].Trim();
                string valueStr = keyValue[1].Trim();

                try
                {
                    T value = (T)System.Convert.ChangeType(valueStr, typeof(T));
                    dict.Set(key, value);
                }
                catch (System.Exception)
                {
                    // If conversion fails, skip this entry
                    // For complex types, you might want to use JSON deserialization instead
                    Debug.LogWarning($"Failed to parse value '{valueStr}' for key '{key}' as type {typeof(T).Name}");
                }
            }
        }
        return dict;
    }

    /// <summary>
    /// Alternative FromString method that accepts a custom converter function for complex types.
    /// </summary>
    public static SerializableDictionary<T> FromString(string data, System.Func<string, T> valueParser)
    {
        var dict = new SerializableDictionary<T>();
        if (string.IsNullOrEmpty(data) || valueParser == null)
            return dict;

        string[] pairs = data.Split(';');
        foreach (string pair in pairs)
        {
            if (string.IsNullOrWhiteSpace(pair))
                continue;

            string[] keyValue = pair.Split('=');
            if (keyValue.Length == 2)
            {
                string key = keyValue[0].Trim();
                string valueStr = keyValue[1].Trim();

                try
                {
                    T value = valueParser(valueStr);
                    dict.Set(key, value);
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning($"Failed to parse value '{valueStr}' for key '{key}': {ex.Message}");
                }
            }
        }
        return dict;
    }
}

}
