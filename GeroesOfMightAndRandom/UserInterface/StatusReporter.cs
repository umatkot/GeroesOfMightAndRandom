using System;

namespace GeroesOfMightAndRandom.UserInterface
{
    public class StatusReporter : IStatusReporter
    {
        public void Clear()
        {
            Console.Clear();
        }

        public void Write(object data)
        {
            Console.Write(data);
        }

        public void WriteLine(object data)
        {
            Console.WriteLine(data);
        }
    }
}
