#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor(typeof(RobotController))]
public class RobotControllerEditor : Editor
{
    private SerializedProperty instructionsProp;
    private List<ReorderableList> commandLists = new();

    private void OnEnable()
    {
        instructionsProp = serializedObject.FindProperty("instructionAssets");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("🤖 Robot Instruction Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        if (instructionsProp.arraySize == 0)
        {
            EditorGUILayout.HelpBox("No instructions assigned. Drag and drop RobotInstructionSOs here.", MessageType.Info);
        }

        for (int i = 0; i < instructionsProp.arraySize; i++)
        {
            SerializedProperty instructionProp = instructionsProp.GetArrayElementAtIndex(i);
            RobotInstructionSO instructionSO = instructionProp.objectReferenceValue as RobotInstructionSO;

            if (instructionSO == null) continue;

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();

            instructionProp.objectReferenceValue = EditorGUILayout.ObjectField(
                $"Instruction {i + 1}", instructionSO, typeof(RobotInstructionSO), false);

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                instructionsProp.DeleteArrayElementAtIndex(i);
                continue;
            }

            EditorGUILayout.EndHorizontal();

            // Nested Command Editor
            SerializedObject instructionSerialized = new SerializedObject(instructionSO);
            SerializedProperty commandsProp = instructionSerialized.FindProperty("commands");

            EditorGUI.indentLevel++;
            for (int j = 0; j < commandsProp.arraySize; j++)
            {
                SerializedProperty cmdProp = commandsProp.GetArrayElementAtIndex(j);
                EditorGUILayout.BeginHorizontal();

                cmdProp.objectReferenceValue = EditorGUILayout.ObjectField(
                    $"  └ Command {j + 1}", cmdProp.objectReferenceValue, typeof(RobotCommandSO), false);

                if (GUILayout.Button("✎", GUILayout.Width(25)))
                {
                    Selection.activeObject = cmdProp.objectReferenceValue;
                }

                if (GUILayout.Button("–", GUILayout.Width(20)))
                {
                    commandsProp.DeleteArrayElementAtIndex(j);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(2);
            if (GUILayout.Button("+ Add Command", GUILayout.Width(150)))
            {
                commandsProp.InsertArrayElementAtIndex(commandsProp.arraySize);
            }

            EditorGUI.indentLevel--;

            instructionSerialized.ApplyModifiedProperties();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("+ Add Instruction", GUILayout.Height(30)))
        {
            instructionsProp.InsertArrayElementAtIndex(instructionsProp.arraySize);
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("🪄 Open Command Creation Wizard", GUILayout.Height(25)))
        {
            RobotCommandWizard.ShowWindow();
        }
    }
}
#endif