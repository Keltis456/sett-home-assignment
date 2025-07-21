
using UnityEngine;

[CreateAssetMenu(menuName = "Robot/Instruction")]
public class RobotInstructionSO : ScriptableObject
{
    public RobotCommandSO[] commands;

    public RobotInstruction ToRuntimeInstruction()
    {
        var runtime = new RobotInstruction();
        foreach (var cmd in commands)
        {
            runtime.AddCommand(cmd.ToRuntimeCommand());
        }
        return runtime;
    }
}
