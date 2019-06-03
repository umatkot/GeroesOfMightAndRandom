namespace GeroesOfMightAndRandom.UserInterface
{
    public interface IUserInput
    {
        UserCommand GetUserCommand(IStatusReporter statusReporter);
        string GetUserExpression();
    }
}