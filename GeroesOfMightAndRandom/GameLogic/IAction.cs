using GeroesOfMightAndRandom.UserInterface;

namespace GeroesOfMightAndRandom.GameLogic
{
    public interface IAction
    {
        void Scene(IUserInput userInput, IStatusReporter statusReporter);
    }
}