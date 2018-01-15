using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ScriptableDataWindow : EditorWindow {

    [MenuItem("Window/ScriptableData generator")]
    public static void OpenWindow()
    {
        var window = EditorWindow.GetWindow<ScriptableDataWindow>();
        window.titleContent = new GUIContent("ScriptableData");
    }

    public void OnGUI() {
        string fileName = "Assets/ScriptableData.asset";
        if (GUILayout.Button("Save asset")) {
            ScriptableData scriptableObject = CreateTestScriptableObject();
            AssetDatabase.CreateAsset(scriptableObject, fileName);
        }
    }

    private static ScriptableData CreateTestScriptableObject() {
        ScriptableData scriptableObject = ScriptableObject.CreateInstance<ScriptableData>();
        scriptableObject.stringProperty = "Test String";
        scriptableObject.intProperty = 1234;
        if (scriptableObject.dictionaryProperty != null) {
            scriptableObject.dictionaryProperty.Clear();
            scriptableObject.dictionaryProperty["key1"] = 111;
            scriptableObject.dictionaryProperty["key2"] = 222;
            scriptableObject.dictionaryProperty["key3"] = 333;
        }
        return scriptableObject;
    }
}
