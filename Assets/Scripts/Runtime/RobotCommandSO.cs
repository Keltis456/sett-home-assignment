
using UnityEngine;

public abstract class RobotCommandSO : ScriptableObject
{
    public float duration = 1f;

    public abstract RobotCommand ToRuntimeCommand();
}
