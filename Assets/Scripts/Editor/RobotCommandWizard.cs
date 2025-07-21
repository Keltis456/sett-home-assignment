#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class RobotCommandWizard : EditorWindow
{
    private enum CommandType { MoveTo, RotateTo, ChangeColor }
    private CommandType selectedType;

    private Vector3 vecValue = Vector3.zero;
    private Color colorValue = Color.white;
    private float duration = 1f;
    private string assetName = "NewCommand";

    [MenuItem("Tools/Robot/Create New Command")]
    public static void ShowWindow()
    {
        GetWindow<RobotCommandWizard>("Command Wizard");
    }

    private void OnGUI()
    {
        GUILayout.Label("🛠 Create Robot Command", EditorStyles.boldLabel);

        EditorGUILayout.Space(10);
        selectedType = (CommandType)EditorGUILayout.EnumPopup("Command Type", selectedType);

        duration = EditorGUILayout.FloatField("Duration (sec)", duration);

        switch (selectedType)
        {
            case CommandType.MoveTo:
                vecValue = EditorGUILayout.Vector3Field("Target Position", vecValue);
                break;
            case CommandType.RotateTo:
                vecValue = EditorGUILayout.Vector3Field("Target Rotation", vecValue);
                break;
            case CommandType.ChangeColor:
                colorValue = EditorGUILayout.ColorField("Target Color", colorValue);
                break;
        }

        EditorGUILayout.Space(10);
        assetName = EditorGUILayout.TextField("Asset Name", assetName);

        EditorGUILayout.Space(20);

        GUI.enabled = duration > 0 && !string.IsNullOrWhiteSpace(assetName);
        if (GUILayout.Button("✅ Create Command", GUILayout.Height(30)))
        {
            CreateCommandAsset();
        }
        GUI.enabled = true;
    }

    private void CreateCommandAsset()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Command", assetName, "asset", "Choose where to save the command");

        if (string.IsNullOrEmpty(path))
            return;

        RobotCommandSO commandSO = null;

        switch (selectedType)
        {
            case CommandType.MoveTo:
                var move = ScriptableObject.CreateInstance<MoveToCommandSO>();
                move.duration = duration;
                move.targetPosition = vecValue;
                commandSO = move;
                break;

            case CommandType.RotateTo:
                var rotate = ScriptableObject.CreateInstance<RotateToCommandSO>();
                rotate.duration = duration;
                rotate.targetRotationEuler = vecValue;
                commandSO = rotate;
                break;

            case CommandType.ChangeColor:
                var color = ScriptableObject.CreateInstance<ChangeColorCommandSO>();
                color.duration = duration;
                color.targetColor = colorValue;
                commandSO = color;
                break;
        }

        if (commandSO != null)
        {
            AssetDatabase.CreateAsset(commandSO, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = commandSO;
            Close();
        }
    }
}
#endif