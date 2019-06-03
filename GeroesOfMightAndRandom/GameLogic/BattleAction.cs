using System.Collections.Generic;
using System.Linq;
using GeroesOfMightAndRandom.Models;
using GeroesOfMightAndRandom.Statics;
using GeroesOfMightAndRandom.UserInterface;

namespace GeroesOfMightAndRandom.GameLogic
{
    public class BattleAction : IAction
    {
        /// <summary>
        /// Активный замок
        /// </summary>
        private Castle SelectedCastle { get; set; }
        private List<Castle> Castles { get; }
        private Units Units { get; }

        
        private int HitChance { get; set; }

        public BattleAction(List<Castle> castles, Units units)
        {
            Castles = castles;
            Units = units;

            /*Замок, который атакует выбирается очень случайным образом*/
            SelectedCastle = castles[Utils.GetSimpleRandom(castles.Count)];
        }

        /// <summary>
        /// Признак того, что битва продолжается
        /// </summary>
        /// <returns></returns>
        public bool IsAction => BattleStat().All(isLife => isLife);

        /// <summary>
        /// Перебирает замки и проверяет, что в каждом замке есть ещё живые юниты
        /// </summary>
        /// <returns></returns>
        private IEnumerable<bool> BattleStat()
        {
            foreach (var castle in Castles)
            {
                yield return Units.ByCastle(castle).All(unit => unit.IsHeartBeat);
            }
        }

        public Dictionary<UserCommand, DoBattleAction> BattleActions = new Dictionary<UserCommand, DoBattleAction>();
        public delegate bool DoBattleAction(Units attackGroup, Units enemyGroup, IStatusReporter statusReporter);
        public List<DoBattleAction> BattleActionHistory;

        /// <summary>
        /// Действие сцены
        /// </summary>
        public void Scene(IUserInput userInput, IStatusReporter statusReporter)
        {
            BattleActionHistory = new List<DoBattleAction>();
            BattleActions = new Dictionary<UserCommand, DoBattleAction>() {
                { UserCommand.Attack,  Attack },
                { UserCommand.GiveUp,  GiveUp },
                { UserCommand.GetStat,  GetStat },
                { UserCommand.NoCommand,  Boo }
            };

            for ( ; ; ){

                foreach(var castle in Castles)
                {
                    UserCommand userCommand = UserCommand.Attack;
                    if (!IsAction) return;

                    do {
                        statusReporter.WriteLine($"ход героев замка {castle} ({Utils.EnumValueToString<CastleOwner>(castle.Owner)})");

                        if (castle.Owner == CastleOwner.User)
                        {
                            userCommand = userInput.GetUserCommand(statusReporter);
                            statusReporter.Clear();
                        }
                        /*пользователь будет делать ходы, пока функция не вернёт false*/
                    }while(BattleActions[userCommand](Units, Units, statusReporter));
                }
            }
        }

        /// <summary>
        /// Атака одной группы героев на другую
        /// </summary>
        /// <param name="attackGroup"></param>
        /// <param name="enemyGroup"></param>
        /// <param name="statusReporter"></param>
        /// <returns>возврат - повторить ход</returns>
        public bool Attack(Units attackGroup, Units enemyGroup, IStatusReporter statusReporter)
        {
            if(attackGroup.First().GroupIndex == enemyGroup.First().GroupIndex)
            {
                statusReporter.WriteLine("Нельзя нанести урон самому себе");
                return false;
            }

            var damage = attackGroup.UnitsOperatoin.GetDamage();
            enemyGroup.ForEach((enemyGroupUnit) => {

                enemyGroupUnit.GetDamage(damage);
            });

            /*Можно дать имена группам и транслировать их тут*/
            /*можно поменять способ доступа к транслятору имён - как удобно*/
            statusReporter.WriteLine($"герои группы {attackGroup.GroupNamesTranslator[attackGroup.First().GroupIndex]} нанесли урон героям группы {enemyGroup.GroupNamesTranslator[enemyGroup.First().GroupIndex]}.");
            if(enemyGroup.All(enemyUnit => false == enemyUnit.IsHeartBeat))
            {
                statusReporter.WriteLine($"замок {enemyGroup.First().Home} теряет группу {enemyGroup.GroupNamesTranslator[enemyGroup.First().GroupIndex].Replace($"({enemyGroup.First().Home})", "").ClearSpaces()}");
            }

            return false;
        }

        /// <summary>
        /// Замок признаёт поражение
        /// </summary>
        /// <param name="attackGroup"></param>
        /// <param name="enemyGroup"></param>
        /// <param name="statusReporter"></param>
        /// <returns>возврат - повторить ход</returns>
        public bool GiveUp(Units attackGroup, Units enemyGroup, IStatusReporter statusReporter)
        {
            statusReporter.WriteLine($"Замок {attackGroup.First().Home} признаёт поражение.");
            foreach (var defeatUnit in Units.ByCastle(attackGroup.First().Home)){
                defeatUnit.Life = 0;
            }

            return false;
        }

        /// <summary>
        /// Выводит статистику по замку
        /// </summary>
        /// <param name="attackGroup"></param>
        /// <param name="enemyGroup"></param>
        /// <param name="statusReporter"></param>
        /// <returns>возврат - повторить ход</returns>
        public bool GetStat(Units attackGroup, Units enemyGroup, IStatusReporter statusReporter)
        {
            attackGroup.GetCastleStatistics(statusReporter);
            return true;
        }

        /// <summary>
        /// Игра в поддавки
        /// </summary>
        /// <param name="attackGroup"></param>
        /// <param name="enemyGroup"></param>
        /// <param name="statusReporter"></param>
        /// <returns>возврат - повторить ход</returns>
        public bool Boo(Units attackGroup, Units enemyGroup, IStatusReporter statusReporter)
        {
            statusReporter.WriteLine($"замок {attackGroup.First().Home} пропускает ход.");
            return false;
        }
    }
}
