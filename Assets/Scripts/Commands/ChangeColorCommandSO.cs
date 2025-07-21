
using UnityEngine;

[CreateAssetMenu(menuName = "Robot/Command/Change Color")]
public class ChangeColorCommandSO : RobotCommandSO
{
    public Color targetColor = Color.white;

    public override RobotCommand ToRuntimeCommand()
    {
        return new ChangeColorCommand(targetColor, duration);
    }
}
