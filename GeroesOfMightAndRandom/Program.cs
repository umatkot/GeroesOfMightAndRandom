using System;
using GeroesOfMightAndRandom.GameLogic;

namespace GeroesOfMightAndRandom
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game(new GameSettings {
                nMinPerGroup = 5,
                nMaxPerGroup = 10,
                nMinUnitsCount = 15,
                nMaxUnitsCount = 150
            });

            game.Init();

            Console.ReadKey();
        }
    }
}
