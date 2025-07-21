
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
