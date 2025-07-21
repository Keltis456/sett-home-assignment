
using System.Collections.Generic;

public class RobotInstruction
{
    public List<RobotCommand> Commands = new();

    public RobotInstruction AddCommand(RobotCommand command)
    {
        Commands.Add(command);
        return this;
    }
}

public static class RobotCommandFactory
{
    public static RobotCommand Create(RobotCommandData data)
    {
        switch (data.type)
        {
            case CommandType.MoveTo:
                return new MoveToCommand(data.targetPosition, data.duration);
            case CommandType.RotateTo:
                return new RotateToCommand(data.targetRotationEuler, data.duration);
            case CommandType.ChangeColor:
                return new ChangeColorCommand(data.targetColor, data.duration);
            default:
                throw new System.ArgumentException($"Unknown command type: {data.type}");
        }
    }
}
