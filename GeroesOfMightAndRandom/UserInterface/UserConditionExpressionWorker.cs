using GeroesOfMightAndRandom.Statics;

namespace GeroesOfMightAndRandom.UserInterface
{
    public class UserExpressionWorker
    {
        public UserDecision GetUserYesNoDecision(IUserInput userInput)
        {
            var testLine = userInput.GetUserExpression().ClearSpaces();

            if (testLine.Equals("да")) return UserDecision.Y;
            if (testLine.Equals("нет")) return UserDecision.N;

            return UserDecision.W;
        }

        public string GetUserCastleDecision(IUserInput userInput)
        {
            return userInput.GetUserExpression().ClearSpaces();
        }
    }
}
