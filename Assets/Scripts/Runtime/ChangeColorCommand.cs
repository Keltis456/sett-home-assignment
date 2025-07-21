using System.Collections;
using UnityEngine;

namespace Runtime
{
    public class ChangeColorCommand : RobotCommand
    {
        private Color targetColor;

        public ChangeColorCommand(Color color, float duration) : base(duration)
        {
            targetColor = color;
        }

        public override IEnumerator Execute(Transform robot, Renderer renderer)
        {
            Color start = renderer.material.color;
            float time = 0f;

            while (time < Duration)
            {
                renderer.material.color = Color.Lerp(start, targetColor, time / Duration);
                time += Time.deltaTime;
                yield return null;
            }

            renderer.material.color = targetColor;
        }
    }
}
