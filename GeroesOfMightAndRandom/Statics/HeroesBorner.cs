using System.Collections.Generic;
using GeroesOfMightAndRandom.Models;

namespace GeroesOfMightAndRandom.Statics
{
    /// <summary>
    /// Рождатель героев
    /// </summary>
    public static class HeroesBorner
    {
        /// <summary>
        /// Описатель героев по типам
        /// </summary>
        private static readonly Dictionary<string, HeroPrototype> GameUnits = new Dictionary<string, HeroPrototype>
        {
            { "Кентавр", new HeroPrototype( 100, 20, 20) },
            { "Эльф"   , new HeroPrototype( 200, 20, 10) },
            { "Пегас"  , new HeroPrototype(  50,  5, 45) },
            { "Скелет" , new HeroPrototype( 150, 15, 15) },
            { "Зомби"  , new HeroPrototype(  50, 20, 10) },
            { "Вампир" , new HeroPrototype( 150, 10, 50) }
        };

        /// <summary>
        /// Получает для героя его урон и остальные параметры
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void GetUnitParams(HeroPrototype unit)
        {
            if (!GameUnits.ContainsKey(unit.Name))
                Error.WrongHeroName();

            Copy(GameUnits[unit.Name], unit);
        }

        /// <summary>
        /// Копирует свойства из родительского объекта в наследуемый
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        private static void Copy(HeroPrototype parent, HeroPrototype child)
        {
            var parentProperties = parent.GetType().GetProperties();

            foreach (var parentProperty in parentProperties)
            {
                var val = parentProperty.GetValue(parent);
                if (val == null) continue;

                parentProperty.SetValue(child, val);
            }
        }
    }
}
