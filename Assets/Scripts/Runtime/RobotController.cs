using System.Collections;
using UnityEngine;

namespace Runtime
{
    public class RobotController : MonoBehaviour
    {
        public RobotInstructionSO[] instructionAssets;
        private new Renderer renderer;

        private void Start()
        {
            renderer = GetComponent<Renderer>();
            StartCoroutine(ExecuteInstructions());
        }

        private IEnumerator ExecuteInstructions()
        {
            foreach (var instructionSO in instructionAssets)
            {
                var instruction = instructionSO.ToRuntimeInstruction();

                foreach (var command in instruction.Commands)
                {
                    yield return StartCoroutine(command.Execute(transform, renderer));
                }
            }
        }
    }
}
