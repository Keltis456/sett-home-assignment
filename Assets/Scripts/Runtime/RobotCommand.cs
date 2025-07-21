using System.Collections;
using UnityEngine;

namespace Runtime
{
    public abstract class RobotCommand
    {
        protected readonly float Duration = 1f;

        protected RobotCommand(float duration)
        {
            Duration = duration;
        }

        public abstract IEnumerator Execute(Transform robot, Renderer renderer);
    }
}
