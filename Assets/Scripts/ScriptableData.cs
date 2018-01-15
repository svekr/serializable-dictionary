using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableData Asset")]
public class ScriptableData : ScriptableObject {
    [SerializeField]
    public string stringProperty;

    [SerializeField]
    public int intProperty;

    [SerializeField]
    public SerializableDictionaryStringInt dictionaryProperty = SerializableDictionaryStringInt.New<SerializableDictionaryStringInt>();
}
