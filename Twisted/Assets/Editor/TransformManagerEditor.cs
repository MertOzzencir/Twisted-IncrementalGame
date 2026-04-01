// #if UNITY_EDITOR
// using UnityEditor;
// using UnityEngine;

// [CustomEditor(typeof(TransformManager))]
// public class TransformManagerEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();
//         TransformManager script = (TransformManager)target;
//         if (GUILayout.Button("Save"))
//             script.Save();
//         if (GUILayout.Button("Load"))
//             script.Load();
//         if (GUILayout.Button("Erase Data"))
//             script.EraseAllData();
//     }
// }
// #endif