using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GeroesOfMightAndRandom.Statics;
using GeroesOfMightAndRandom.UserInterface;

namespace GeroesOfMightAndRandom.Models
{
    public class Units : List<HeroUnit>, IUnitsOperation
    {
        /// <summary>
        /// Выбирает только тех юнитов, которые располагаются в заданном замке
        /// </summary>
        /// <param name="castle"></param>
        /// <returns></returns>
        public IUnitsOperation ByCastle(Castle castle)
        {
            return new Units(this.Where(unit => unit.Home.Equals(castle)).ToList());
        }
                
        /// <summary>
        /// Для работы с группами юнитов - имена групп, если в будущем потребуется
        /// </summary>
        public ConcurrentDictionary<Guid, string> GroupNamesTranslator { get; set; }

        public Units(List<HeroUnit> thisUnits) {
            AddRange(thisUnits);
            UnitsOperatoin = this;
        }

        public Units() {
            UnitsOperatoin = this;
        }

        /// <summary>
        /// Инициализация параметров для героев - раздача жизней и уронов
        /// </summary>
        public void InitHeroParameters() {
            for(var nUnit = 0; nUnit < Count; nUnit++)
            {
                HeroesBorner.GetUnitParams(this[nUnit]);
            }
        }

        /// <summary>
        /// Набивает героев в отряды
        /// </summary>
        public void FillGroups(int nMinPerGroup, int nMaxPerGroup)
        {
            GroupNamesTranslator = new ConcurrentDictionary<Guid, string>();
            var unitsByCastles = this.GroupBy(units => units.Home);

            var rnd = new Random();

            foreach (var unitsByOneCastle in unitsByCastles)
            {

                var nGroup = Guid.NewGuid();
                var lowBound = 0;
                var namedGroupIndex = 1;

                do
                {
                    /*Имена групп станартные "число (наименование замка)"*/
                    GroupNamesTranslator[nGroup] = $"{namedGroupIndex.ToString()} ({unitsByOneCastle.First().Home})";

                    var nUnitsForNextGroup = rnd.Next(nMinPerGroup, nMaxPerGroup);
                    var unitsForGroup = unitsByOneCastle.Skip(lowBound).Take(nUnitsForNextGroup);
                    if (!unitsForGroup.Any()) break;

                    lowBound += nUnitsForNextGroup;

                    foreach (var unitPerGroup in unitsForGroup)
                    {
                        unitPerGroup.GroupIndex = nGroup;
                    }

                    nGroup = Guid.NewGuid();
                } while (true);

                do
                {
                    /*Проверка - всё-таки группы не комплектуются до конца*/
                    /*нужно для той группы, где недобор выбрать элемент из группы, в которой больше всего героев*/
                    var groupsWithMinElements = unitsByOneCastle.GroupBy(unit => unit.GroupIndex).Where(groupUnits => groupUnits.Count() < nMinPerGroup);
                    if (!groupsWithMinElements.Any()) break;

                    foreach (var groupWithMinElements in unitsByOneCastle.GroupBy(unit => unit.GroupIndex).Where(groupUnits => groupUnits.Count() < nMinPerGroup))
                    {
                        var groupsWithMaxElements = unitsByOneCastle
                            .GroupBy(unit => unit.GroupIndex)
                            .Where(groupUnits => groupUnits.Count() > nMinPerGroup);

                        /*героев брать неоткуда - группы не сформированы - вероятно, некачественно сгенерированы герои*/
                        if (!groupsWithMaxElements.Any()) return;

                        /*отряд с самым большим количеством героев*/
                        var groupWithMaxElements = groupsWithMaxElements.Aggregate((c, v) => c.Count() < v.Count() ? c : v);

                        groupWithMaxElements.First().GroupIndex = groupWithMinElements.First().GroupIndex;
                    }
                    /*Делать до тех пор, пока все группы с элементами, количество которых меньше nMinPerGroup не будут обработаны*/
                } while (true);
            }
        }

        /// <summary>
        /// Отображает содержание по группировкам (замок, группы)
        /// </summary>
        /// <param name="statusReporter"></param>
        public void ShowNamesGroup(IStatusReporter statusReporter) {

            statusReporter.WriteLine($"Замок {this.First().Home}");
            statusReporter.WriteLine($"воинов {Count}");

            var unitsGroups = this.ToLookup(unit => unit.GroupIndex);

            statusReporter.WriteLine($"Группировок в замке {unitsGroups.Count()}");
            
            foreach (var unitGroup in unitsGroups.OrderBy(ug => ug.Key))
            {
                statusReporter.WriteLine($"{unitGroup.Key}) {unitGroup.Select(unit => unit.ToString()).Aggregate((u,g) => $"{u}, {g}")}");
            }
        }

        /// <summary>
        /// Подсчитывает урон группировки
        /// </summary>
        /// <param name="attackGroup"></param>
        /// <returns></returns>
        double IUnitsOperation.GetDamageGroup(Guid attackGroup)
        {
            return this.Sum(unit => unit.GroupIndex == attackGroup ? unit.Damage : 0);
        }

        public IUnitsOperation UnitsOperatoin;

        /// <summary>
        /// Подсчитывает урон
        /// </summary>
        /// <param name="attackGroup"></param>
        /// <returns></returns>
        double IUnitsOperation.GetDamage()
        {
            return this.Sum(unit => unit.Damage);
        }

        /// <summary>
        /// Подсчитывает защиту
        /// </summary>
        /// <returns></returns>
        double IUnitsOperation.GetProtectionLevel()
        {
            return this.Sum(unit => unit.Life);
        }

        /// <summary>
        /// Подсчитывает защиту группировки
        /// </summary>
        /// <returns></returns>
        double IUnitsOperation.GetGroupProtectionLevel(Guid attackGroup)
        {
            return this.Sum(unit => unit.GroupIndex == attackGroup ? unit.Life : 0);
        }
    }
}
