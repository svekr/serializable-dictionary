using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class PairData {
    public SerializableTemplate serializableTemplate;
    public SerializedObject serializedObject;
}

public class SerializableTemplate : ScriptableObject {
    public string key;
    public int value = 0;
}

[CustomEditor(typeof(ScriptableData))]
public class ScriptableDataInspector : Editor {

    private PairData _initialPair;
    private string _deleteKey = null;
    private string _addKey = null;

    public override void OnInspectorGUI() {
        List<PairData> items = new List<PairData>();
        var scriptableData = (ScriptableData)target;
        var localSerializedObject = new SerializedObject(target);

        _deleteKey = null;
        _addKey = null;
        if (_initialPair == null) {
            _initialPair = new PairData();
            _initialPair.serializableTemplate = GetTemplate();
        }

        SerializedProperty stringProperty = localSerializedObject.FindProperty(GetMemberName(() => scriptableData.stringProperty));
        SerializedProperty intProperty = localSerializedObject.FindProperty(GetMemberName(() => scriptableData.intProperty));
        SerializedProperty dictionaryProperty = localSerializedObject.FindProperty(GetMemberName(() => scriptableData.dictionaryProperty));

        EditorGUILayout.PropertyField(stringProperty);
        EditorGUILayout.PropertyField(intProperty);

        EditorGUI.BeginChangeCheck();

        if (dictionaryProperty != null) {
            dictionaryProperty.isExpanded = EditorGUILayout.Foldout(
                dictionaryProperty.isExpanded,
                dictionaryProperty.displayName
            );
            if (dictionaryProperty.isExpanded) {
                UpdateInitialPair(scriptableData);
                foreach (var keyValuePair in scriptableData.dictionaryProperty) {
                    items.Add(GetDictionaryPair(keyValuePair.Key, keyValuePair.Value));
                }
            }
        }

        if (EditorGUI.EndChangeCheck()) {
            _initialPair.serializedObject.ApplyModifiedProperties();
            int itemsCount = items.Count;
            if (_deleteKey != null && scriptableData.dictionaryProperty.ContainsKey(_deleteKey)) {
                scriptableData.dictionaryProperty.Remove(_deleteKey);
            }
            for (int i = 0; i < itemsCount; i++) {
                PairData pairData = items[i];
                pairData.serializedObject.ApplyModifiedProperties();
                string pairKey = pairData.serializableTemplate.key;
                if (pairKey != _deleteKey && (scriptableData.dictionaryProperty.ContainsKey(pairKey) || pairKey == _addKey)) {
                    scriptableData.dictionaryProperty[pairKey] = pairData.serializableTemplate.value;
                }
            }
        }

        localSerializedObject.ApplyModifiedProperties();
    }

    protected void UpdateInitialPair(ScriptableData target) {
        _initialPair.serializedObject = new SerializedObject(_initialPair.serializableTemplate);
        SerializedProperty keyProperty = _initialPair.serializedObject.FindProperty("key");
        SerializedProperty valueProperty = _initialPair.serializedObject.FindProperty("value");
        float viewWidth = EditorGUIUtility.currentViewWidth * 0.5f - 40;
        GUILayout.BeginHorizontal();
        EditorGUILayout.DelayedTextField(keyProperty, GUIContent.none, GUILayout.Width(viewWidth));
        EditorGUILayout.DelayedIntField(valueProperty, GUIContent.none, GUILayout.Width(viewWidth));
        if (GUILayout.Button("Add", GUILayout.Width(50))) {
            _initialPair.serializedObject.ApplyModifiedProperties();
            if (_initialPair.serializableTemplate.key != null && _initialPair.serializableTemplate.key.Length > 0) {
                target.dictionaryProperty[_initialPair.serializableTemplate.key] = _initialPair.serializableTemplate.value;
                _addKey = (string)_initialPair.serializableTemplate.key.Clone();
                _initialPair.serializableTemplate.key = "";
                _initialPair.serializableTemplate.value = 0;
            }
        }
        GUILayout.EndHorizontal();
    }

    protected PairData GetDictionaryPair(string key, int value) {
        PairData pairData = new PairData();
        pairData.serializableTemplate = GetTemplate();
        pairData.serializableTemplate.key = key;
        pairData.serializableTemplate.value = value;
        pairData.serializedObject = new SerializedObject(pairData.serializableTemplate);
        SerializedProperty keyProperty = pairData.serializedObject.FindProperty("key");
        SerializedProperty valueProperty = pairData.serializedObject.FindProperty("value");
        float viewWidth = EditorGUIUtility.currentViewWidth * 0.5f - 40;
        GUILayout.BeginHorizontal();
        EditorGUILayout.DelayedTextField(keyProperty, GUIContent.none, GUILayout.Width(viewWidth));
        EditorGUILayout.DelayedIntField(valueProperty, GUIContent.none, GUILayout.Width(viewWidth));
        if (GUILayout.Button("Delete", GUILayout.Width(50))) {
            _deleteKey = key;
        }
        GUILayout.EndHorizontal();
        return pairData;
    }

    protected SerializableTemplate GetTemplate() {
        return ScriptableObject.CreateInstance<SerializableTemplate>();
    }

    public static string GetMemberName<TValue>(Expression<Func<TValue>> memberAccess) {
        return ((MemberExpression)memberAccess.Body).Member.Name;
    }
}
