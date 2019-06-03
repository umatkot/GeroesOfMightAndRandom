using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GeroesOfMightAndRandom.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class HeroTests
    {
        [TestMethod]
        public void SelectorByCastleShouldGetOnlyCastleUnits()
        {
            var castles = new List<Castle>
            {
                new Castle("Оплот", new [] {"Кентавр", "Эльф", "Пегас"}),
                new Castle("Некрополис", new [] {"Скелет", "Зомби", "Вампир"})
            };

            var units = new Units
            {
                new HeroUnit(castles, "Кентавр"),
                new HeroUnit(castles, "Эльф"),
                new HeroUnit(castles, "Пегас"),

                new HeroUnit(castles, "Скелет"),
                new HeroUnit(castles, "Зомби"),
                new HeroUnit(castles, "Вампир")
            };

            var someCastle = castles.First();
            var elements = units.ByCastle(castles.First()).Select(unit => unit.Home);
            elements.First().Id = Guid.NewGuid();
            elements.Should().AllBeEquivalentTo(someCastle);
        }
    }
}
