using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableDictionaryTemplate<TKey, TValue> : Dictionary<TKey, TValue> {
    [SerializeField]
    public Dictionary<TKey, TValue> dictionary;

    [HideInInspector]
    [SerializeField]
    private List<TKey> _keys;

    [HideInInspector]
    [SerializeField]
    private List<TValue> _values;

    static public T New<T>() where T : SerializableDictionaryTemplate<TKey, TValue>, new() {
        var result = new T();
        result.dictionary = new Dictionary<TKey, TValue>();
        return result;
    }

    public void OnBeforeSerialize() {
        _keys = new List<TKey>();
        _values = new List<TValue>();

        foreach (KeyValuePair<TKey, TValue> keyValuePair in this) {
            _keys.Add(keyValuePair.Key);
            _values.Add(keyValuePair.Value);
        }
    }

    public void OnAfterDeserialize() {
        if (_keys != null && _values != null) {
            int itemsCount = Mathf.Min(_keys.Count, _values.Count);
            dictionary = new Dictionary<TKey, TValue>(itemsCount);
            for (int i = 0; i < itemsCount; i++) {
                this[_keys[i]] = _values[i];
            }
        }
    }
}
