
using System.Collections;
using UnityEngine;

public class MoveToCommand : RobotCommand
{
    private Vector3 targetPosition;

    public MoveToCommand(Vector3 targetPosition, float duration) : base(duration)
    {
        this.targetPosition = targetPosition;
    }

    public override IEnumerator Execute(Transform robot, Renderer renderer)
    {
        Vector3 start = robot.position;
        float time = 0f;

        while (time < Duration)
        {
            robot.position = Vector3.Lerp(start, targetPosition, time / Duration);
            time += Time.deltaTime;
            yield return null;
        }

        robot.position = targetPosition;
    }
}
