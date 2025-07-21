using System;
using UnityEngine;

namespace Runtime
{
    [Serializable]
    public class RobotCommandData
    {
        public CommandType type;
        public float duration = 1f;
        public Vector3 targetPosition; // For MoveTo
        public Vector3 targetRotationEuler; // For RotateTo
        public Color targetColor = Color.white; // For ChangeColor
    }

    public enum CommandType
    {
        MoveTo,
        RotateTo,
        ChangeColor
    }

    [CreateAssetMenu(menuName = "Robot/Instruction")]
    public class RobotInstructionSO : ScriptableObject
    {
        public RobotCommandData[] commands;

        public RobotInstruction ToRuntimeInstruction()
        {
            var runtime = new RobotInstruction();
            foreach (var cmdData in commands)
            {
                runtime.AddCommand(RobotCommandFactory.Create(cmdData));
            }
            return runtime;
        }
    }
}