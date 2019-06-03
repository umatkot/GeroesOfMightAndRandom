using System;
using System.Collections.Generic;
using GeroesOfMightAndRandom.UserInterface;

namespace GeroesOfMightAndRandom.Models
{
    public interface IUnitsOperation : IList<HeroUnit>
    {
        double GetDamage();
        double GetDamageGroup(Guid attackGroup);

        double GetProtectionLevel();
        double GetGroupProtectionLevel(Guid attackGroup);

        void ShowNamesGroup(IStatusReporter statusReporter);
    }
}
