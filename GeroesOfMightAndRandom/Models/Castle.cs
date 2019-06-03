using System.Collections.Generic;
using System;
using System.Linq;
using GeroesOfMightAndRandom.UserInterface;

namespace GeroesOfMightAndRandom.Models
{
    public class Castle
    {
        public CastleOwner Owner { get; set; }

        public Guid Id { get; set; }

        /// <summary>
        /// Наименование замка
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Статистика по замку
        /// </summary>
        private Dictionary<string, int> Units { get; }

        /// <summary>
        /// конструктор для инициализации
        /// </summary>
        /// <param name="name"></param>
        /// <param name="units">указывает, какие герои могут размещаться в замке</param>
        public Castle(string name, string [] units)
        {
            Id = Guid.NewGuid();
            
            /*Ставится для всех замков по-умолчанию*/
            Owner = CastleOwner.Ai;

            Name = name;
            Units = units.ToDictionary(unit => unit, unit => 0);
        }

        /// <summary>
        /// Возвращает список тех юнитов, которые могут располагаться данном замке
        /// </summary>
        /// <returns></returns>
        public string[] GetAvailableUnits() {
            return Units.Keys.ToArray();
        }

        /// <summary>
        /// конструктор для теста
        /// </summary>
        /// <param name="name"></param>
        public Castle(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        /// <summary>
        /// Можно ли добавить юнита в замок или нет
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CanAdd(string name)
        {
            return Units.ContainsKey(name);
        }

        public bool Equals(Castle castleObj)
        {
            return Name.ToLower().Equals(castleObj.Name.ToLower());
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
