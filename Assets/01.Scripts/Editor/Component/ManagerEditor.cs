// using JMT.Manager.Core;
// using UnityEngine;

// namespace MIE.Editor.Component
// {
//     [UnityEditor.CustomEditor(typeof(Managers), true)]
//     public class ManagerEditor : UnityEditor.Editor
//     {
//         public override void OnInspectorGUI()
//         {
//             DrawDefaultInspector();

//             var managers = (Managers)target;
//             if (managers == null) return;

//             GUILayoutExtension.DrawHorizontalLine(Color.white);

//             GUILayout.Label("Registered Managers:");

//             int index = 0;
//             foreach (var managerDatasManager in managers.ManagerDatas.Managers)
//             {
//                 GUILayout.Label($"{index}    {managerDatasManager.Key.Name}");
//                 index++;
//             }
//         }
//     }
// }
