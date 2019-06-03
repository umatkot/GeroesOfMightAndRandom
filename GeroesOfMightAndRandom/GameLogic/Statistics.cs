using System.Linq;
using GeroesOfMightAndRandom.Models;
using GeroesOfMightAndRandom.UserInterface;

namespace GeroesOfMightAndRandom.GameLogic
{
    public static class Statistics
    {
        public static void GetCastleStatistics(this Units units, IStatusReporter statusReporter)
        {
            statusReporter.WriteLine($"общая сила замка {units.UnitsOperatoin.GetDamage()}");
            statusReporter.WriteLine($"общая защита замка {units.UnitsOperatoin.GetProtectionLevel()}");
            foreach(var unitGroup in units.GroupBy(unit => unit.GroupIndex))
            {
                statusReporter.WriteLine($"группировка {unitGroup.First().GroupIndex}");
                statusReporter.WriteLine($"общая сила {units.UnitsOperatoin.GetDamageGroup(unitGroup.First().GroupIndex)}");
                statusReporter.WriteLine($"общая защита {units.UnitsOperatoin.GetGroupProtectionLevel(unitGroup.First().GroupIndex)}");
            }
        }
    }
}
