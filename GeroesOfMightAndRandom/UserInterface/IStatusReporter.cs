namespace GeroesOfMightAndRandom.UserInterface
{
    public interface IStatusReporter
    {
        void WriteLine(object data);
        void Write(object data);
        void Clear();
    }
}