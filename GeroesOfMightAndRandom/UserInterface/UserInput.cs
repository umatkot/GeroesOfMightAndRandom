using System;

namespace GeroesOfMightAndRandom.UserInterface
{
    public class ConsoleUserInput : IUserInput
    {
        public UserCommand GetUserCommand(IStatusReporter statusReporter)
        {
            statusReporter.WriteLine("Введите комманду");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.A:
                    return UserCommand.Attack;
                case ConsoleKey.S:
                    return UserCommand.GetStat;
                case ConsoleKey.U:
                    return UserCommand.GiveUp;

                default:
                    return UserCommand.NoCommand;
            }
        }

        public string GetUserExpression()
        {
            return Console.ReadLine();
        }
    }
}
