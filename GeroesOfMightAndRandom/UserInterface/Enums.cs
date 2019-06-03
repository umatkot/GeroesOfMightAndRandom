namespace GeroesOfMightAndRandom.UserInterface
{
    public enum UserCommand
    {
        Attack,
        GetStat,
        GiveUp,
        NoCommand
    }

    public enum UserDecision
    {
        Y, N,
        /// <summary>
        /// Wrong variant
        /// </summary>
        W
    }

    public enum CastleOwner
    {
        User, Ai
    }
}
