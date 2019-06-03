using System;
using System.Collections.Generic;

namespace GeroesOfMightAndRandom.Models
{
    public class HeroUnitList : List<HeroUnit>
    {
        public List<HeroUnit> Where(Func<HeroUnit, bool> where)
        {
            return Where(where);
        }

        public List<TResult> Select<TSource, TResult>(Func<TSource, int, TResult> select)
        {
            return Select(select);
        }

        public bool All(Func<HeroUnit, bool> all)
        {
            return All(all);
        }
    }
}
