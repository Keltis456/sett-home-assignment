#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using Runtime;

[CustomEditor(typeof(RobotController))]
public class RobotControllerEditor : Editor
{
    private SerializedProperty instructionsProp;

    private void OnEnable()
    {
        instructionsProp = serializedObject.FindProperty("instructionAssets");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.HelpBox("⚠️ Changes to any instruction will affect ALL robots that use it!", MessageType.Warning);
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

            // Move Up button
            GUI.enabled = i > 0;
            if (GUILayout.Button("↑", GUILayout.Width(20)))
            {
                instructionsProp.MoveArrayElement(i, i - 1);
            }
            // Move Down button
            GUI.enabled = i < instructionsProp.arraySize - 1;
            if (GUILayout.Button("↓", GUILayout.Width(20)))
            {
                instructionsProp.MoveArrayElement(i, i + 1);
            }
            GUI.enabled = true;

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                instructionsProp.DeleteArrayElementAtIndex(i);
                continue;
            }

            EditorGUILayout.EndHorizontal();

            DrawCommandListEditor(new SerializedObject(instructionSO));

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        // Add Instruction: choose existing or create new
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Add Instruction", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+ Create New Instruction", GUILayout.Height(30)))
        {
            string path = EditorUtility.SaveFilePanelInProject("Create New Instruction", "New Robot Instruction", "asset", "Choose location for new instruction asset");
            if (!string.IsNullOrEmpty(path))
            {
                var newInstruction = ScriptableObject.CreateInstance<RobotInstructionSO>();
                AssetDatabase.CreateAsset(newInstruction, path);
                AssetDatabase.SaveAssets();
                instructionsProp.InsertArrayElementAtIndex(instructionsProp.arraySize);
                instructionsProp.GetArrayElementAtIndex(instructionsProp.arraySize - 1).objectReferenceValue = newInstruction;
            }
        }
        if (GUILayout.Button("+ Reuse Existing Instruction", GUILayout.Height(30)))
        {
            EditorGUIUtility.ShowObjectPicker<RobotInstructionSO>(null, false, "", 12345);
        }
        EditorGUILayout.EndHorizontal();

        // Handle object picker for reusing existing instruction
        if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "ObjectSelectorClosed")
        {
            Object picked = EditorGUIUtility.GetObjectPickerObject();
            if (picked is RobotInstructionSO pickedInstruction)
            {
                instructionsProp.InsertArrayElementAtIndex(instructionsProp.arraySize);
                instructionsProp.GetArrayElementAtIndex(instructionsProp.arraySize - 1).objectReferenceValue = pickedInstruction;
                serializedObject.ApplyModifiedProperties();
                GUI.FocusControl(null); // Remove focus to avoid repeated assignment
                Event.current.Use(); // Prevent further processing
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    public static void DrawCommandListEditor(SerializedObject instructionSerialized)
    {
        SerializedProperty commandsProp = instructionSerialized.FindProperty("commands");
        EditorGUI.indentLevel++;
        for (int j = 0; j < commandsProp.arraySize; j++)
        {
            SerializedProperty cmdProp = commandsProp.GetArrayElementAtIndex(j);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"  └ Command {j + 1}", EditorStyles.boldLabel);
            
            // Move Up button
            GUI.enabled = j > 0;
            if (GUILayout.Button("↑", GUILayout.Width(20)))
            {
                commandsProp.MoveArrayElement(j, j - 1);
            }
            GUI.enabled = j < commandsProp.arraySize - 1;
            if (GUILayout.Button("↓", GUILayout.Width(20)))
            {
                commandsProp.MoveArrayElement(j, j + 1);
            }
            GUI.enabled = true;

            if (GUILayout.Button("–", GUILayout.Width(20)))
            {
                commandsProp.DeleteArrayElementAtIndex(j);
                break;
            }
            EditorGUILayout.EndHorizontal();

            SerializedProperty typeProp = cmdProp.FindPropertyRelative("type");
            SerializedProperty durationProp = cmdProp.FindPropertyRelative("duration");
            SerializedProperty posProp = cmdProp.FindPropertyRelative("targetPosition");
            SerializedProperty rotProp = cmdProp.FindPropertyRelative("targetRotationEuler");
            SerializedProperty colorProp = cmdProp.FindPropertyRelative("targetColor");

            EditorGUILayout.PropertyField(typeProp);
            EditorGUILayout.PropertyField(durationProp);

            CommandType type = (CommandType)typeProp.enumValueIndex;
            switch (type)
            {
                case CommandType.MoveTo:
                    EditorGUILayout.PropertyField(posProp, new GUIContent("Target Position"));
                    break;
                case CommandType.RotateTo:
                    EditorGUILayout.PropertyField(rotProp, new GUIContent("Target Rotation"));
                    break;
                case CommandType.ChangeColor:
                    EditorGUILayout.PropertyField(colorProp, new GUIContent("Target Color"));
                    break;
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space(2);
        if (GUILayout.Button("+ Add Command", GUILayout.Width(150)))
        {
            commandsProp.InsertArrayElementAtIndex(commandsProp.arraySize);
        }
        EditorGUI.indentLevel--;
        instructionSerialized.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(RobotInstructionSO))]
public class RobotInstructionSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("⚠️ Changes to this instruction will affect ALL robots that use it!", MessageType.Warning);
        serializedObject.Update();
        RobotControllerEditor.DrawCommandListEditor(serializedObject);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif