using System.Collections;
using UnityEngine;

namespace Runtime.Commands
{
    public class RotateToCommand : RobotCommand
    {
        private readonly Quaternion targetRotation;

        public RotateToCommand(Vector3 eulerAngles, float duration) : base(duration)
        {
            targetRotation = Quaternion.Euler(eulerAngles);
        }

        public override IEnumerator Execute(Transform robot, Renderer renderer)
        {
            var start = robot.rotation;
            var time = 0f;

            while (time < Duration)
            {
                robot.rotation = Quaternion.Slerp(start, targetRotation, time / Duration);
                time += Time.deltaTime;
                yield return null;
            }

            robot.rotation = targetRotation;
        }
    }
}
