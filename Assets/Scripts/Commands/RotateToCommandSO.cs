
using UnityEngine;

[CreateAssetMenu(menuName = "Robot/Command/Rotate To")]
public class RotateToCommandSO : RobotCommandSO
{
    public Vector3 targetRotationEuler;

    public override RobotCommand ToRuntimeCommand()
    {
        return new RotateToCommand(targetRotationEuler, duration);
    }
}
