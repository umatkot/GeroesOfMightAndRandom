using System;
using System.Collections.Generic;
using GeroesOfMightAndRandom.Statics;

namespace GeroesOfMightAndRandom.Models
{
    /// <summary>
    /// Версия для поведения героя
    /// </summary>
    public class HeroUnit : HeroPrototype
    {
        /// <summary>
        /// Принадлежность героя к замку - один замок -> куча героев
        /// </summary>
        public Castle Home { get; set; }
        
        /// <summary>
        /// Принадлежность юнита к группе - одна группа -> куча героев
        /// Выбирается тип Guid для того, чтобы можно было напрямую получать юнитов группы
        /// иначе следовало бы получать доступ к тем героям, которые находятся в замке и искать уже там группу
        /// </summary>
        public Guid GroupIndex { get; set; }

        public bool IsHeartBeat => Life > 0;

        public HeroUnit(List<Castle> castles, string name) : base(0,0,0)
        {
            Name = name;
            Home = castles.Find(c => c.CanAdd(Name));

            if (Home == null)
                Error.CastleNotFoundForUnit();
        }

        public void GetDamage(double damage)
        {
            if (!IsHeartBeat) return;
            Life -= damage;
        }
    }
}
