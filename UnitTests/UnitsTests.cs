using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GeroesOfMightAndRandom.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitsTests
    {
        [TestMethod]
        public void UnitsFillGroupsShowldCreateGroupsWithHeroesAtRangeFrom5To10Units()
        {
            var castles = new List<Castle>
            {
                new Castle("Оплот", new [] {"Кентавр", "Эльф", "Пегас"})                
            };

            var name = castles.First().GetAvailableUnits().First();

            var units = new Units();

            for (int aUnit = 0; aUnit < 20; aUnit++)
            {
                units.Add(new HeroUnit(castles, name));
            }

            units.FillGroups(5, 10);

            units
                .GroupBy(unit => unit.GroupIndex)
                .All(unitGroup => unitGroup.Count() >= 5 && unitGroup.Count() <= 10)
                .Should().BeTrue();
        }
    }
}
