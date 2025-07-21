using System.Collections;
using UnityEngine;

namespace Runtime.Commands
{
    public class MoveToCommand : RobotCommand
    {
        private readonly Vector3 targetPosition;

        public MoveToCommand(Vector3 targetPosition, float duration) : base(duration)
        {
            this.targetPosition = targetPosition;
        }

        public override IEnumerator Execute(Transform robot, Renderer renderer)
        {
            var start = robot.position;
            var time = 0f;

            while (time < Duration)
            {
                robot.position = Vector3.Lerp(start, targetPosition, time / Duration);
                time += Time.deltaTime;
                yield return null;
            }

            robot.position = targetPosition;
        }
    }
}
