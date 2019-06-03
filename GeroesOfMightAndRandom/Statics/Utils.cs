using System;
using System.Collections.Generic;

namespace GeroesOfMightAndRandom.Statics
{
    public static class Utils
    {
        public static double GetDoubleRandom(double nMaxValue)
        {
            return new Random(DateTime.Now.Millisecond).NextDouble() * nMaxValue / 2;
        }

        public static int GetSimpleRandom(int nMaxValue)
        {
            return new Random(DateTime.Now.Millisecond).Next(0, nMaxValue);
        }

        public static string EnumValueToString<T>(object value)
        {
            return Enum.GetName(typeof(T), value);
        }
    }

    public static class Error
    {
        public static void WrongHeroName()
        {
            throw new KeyNotFoundException("имя героя не распознано");
        }

        public static void CastleNotFoundForUnit()
        {
            throw new Exception("Нет подходящего замка для героя");
        }
    }
}
