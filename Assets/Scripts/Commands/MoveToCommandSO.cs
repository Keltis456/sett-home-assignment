
using UnityEngine;

[CreateAssetMenu(menuName = "Robot/Command/Move To")]
public class MoveToCommandSO : RobotCommandSO
{
    public Vector3 targetPosition;

    public override RobotCommand ToRuntimeCommand()
    {
        return new MoveToCommand(targetPosition, duration);
    }
}
