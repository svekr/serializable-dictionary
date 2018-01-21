using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableData Asset")]
public class ScriptableData : ScriptableObject, ISerializationCallbackReceiver {
    [SerializeField]
    public string stringProperty;

    [SerializeField]
    public int intProperty;

    [SerializeField]
    public SerializableDictionaryStringInt dictionaryProperty = SerializableDictionaryStringInt.New<SerializableDictionaryStringInt>();

    public void OnBeforeSerialize() {
        if (dictionaryProperty != null) {
            dictionaryProperty.OnBeforeSerialize();
        }
    }

    public void OnAfterDeserialize() {
        if (dictionaryProperty != null) {
            dictionaryProperty.OnAfterDeserialize();
        }
    }
}
