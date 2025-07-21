
using System.Collections;
using UnityEngine;

public abstract class RobotCommand
{
    public float Duration;

    protected RobotCommand(float duration)
    {
        Duration = duration;
    }

    public abstract IEnumerator Execute(Transform robot, Renderer renderer);
}
