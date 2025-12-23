using System.Collections.Generic;

public class BiDictionary<TKey, TValue>
{
    private readonly Dictionary<TKey, TValue> forward = new();
    private readonly Dictionary<TValue, TKey> reverse = new();

    public void Add(TKey key, TValue value)
    {
        forward.Add(key, value);
        reverse.Add(value, key);
    }

    public bool TryGetValue(TKey key, out TValue value)
        => forward.TryGetValue(key, out value);

    public bool TryGetKey(TValue value, out TKey key)
        => reverse.TryGetValue(value, out key);

    public bool ContainsKey(TKey key) => forward.ContainsKey(key);
    public bool ContainsValue(TValue value) => reverse.ContainsKey(value);

    public void RemoveByKey(TKey key)
    {
        if (forward.TryGetValue(key, out var val))
        {
            forward.Remove(key);
            reverse.Remove(val);
        }
    }

    public void RemoveByValue(TValue value)
    {
        if (reverse.TryGetValue(value, out var key))
        {
            reverse.Remove(value);
            forward.Remove(key);
        }
    }
}