using System;
using System.Collections.Generic;
using System.Linq;
using GeroesOfMightAndRandom.Models;
using GeroesOfMightAndRandom.UserInterface;

namespace GeroesOfMightAndRandom.GameLogic
{
    public class GameSettings
    {
        /// <summary>
        /// Минимальное допустимое количество героев вообще в игре
        /// </summary>
        public int nMinUnitsCount { get; set; }

        /// <summary>
        /// Максимальное допустимое количество героев вообще в игре
        /// </summary>
        public int nMaxUnitsCount { get; set; }

        /// <summary>
        /// Минимальное количество героев в отряде
        /// </summary>
        public int nMinPerGroup { get; set; }

        /// <summary>
        /// Максимальное количество героев в отряде
        /// </summary>
        public int nMaxPerGroup { get; set; }
    }

    public class Game
    {
        /// <summary>
        /// Доступные замки для игры
        /// </summary>
        private List<Castle> Castles { get; set; }

        private IStatusReporter StatusReporter { get; set; }
        private IUserInput UserInput { get; set; }
        private UserExpressionWorker ExpressionWorker { get; set; }
        private GameSettings GameSettings { get; set; }
        private IAction GameAction { get; set; }

        /// <summary>
        /// Отряды героев, которые будут располагаться в замках и воевать
        /// </summary>
        private Units HeroUnits { get; set; }

        public Game(GameSettings settings)
        {
            ExpressionWorker = new UserExpressionWorker();
            UserInput = new ConsoleUserInput();
            StatusReporter = new StatusReporter();
            GameSettings = settings as GameSettings;
        }

        /// <summary>
        /// Инициациализация игры
        /// </summary>
        public void Init()
        {
            Castles = new List<Castle>
            {
                new Castle("Оплот", new [] {"Кентавр", "Эльф", "Пегас"}),
                new Castle("Некрополис", new [] {"Скелет", "Зомби", "Вампир"})
            };

            HeroUnits = new Units();

            StatusReporter.WriteLine("Пользователь должнен выбрать замок, за который он будет играть.");
            StatusReporter.WriteLine($"Варианты для выбора:");
            Castles.ForEach((castle) => { StatusReporter.WriteLine(castle); });

            GenerateUnits();
        }

        /// <summary>
        /// Инициализация героев замка
        /// </summary>
        public void GenerateUnits()
        {
            var names = Castles.SelectMany(castle => castle.GetAvailableUnits()).ToArray();

            var randomUnits = new Random();

            bool generationDone;

            do
            {
                HeroUnits.Clear();
                
                for (int aUnit = 0; aUnit < randomUnits.Next(GameSettings.nMinUnitsCount, GameSettings.nMaxUnitsCount); aUnit++)
                {
                    HeroUnits.Add(new HeroUnit(Castles, names[randomUnits.Next(0, names.Count())]));
                }

                generationDone = true;

                foreach (var unitsByCastle in HeroUnits.GroupBy(unit => unit.Home))
                {
                    var unitsByCastlesCount = unitsByCastle.Count();
                    if (unitsByCastlesCount < GameSettings.nMinPerGroup)
                    {
                        generationDone = false;
                        /*неудачый random*/
                        StatusReporter.WriteLine($"В замке {unitsByCastle.Key} недобор героев. невозможно сформировать ни одного отряда.");

                        do
                        {
                            StatusReporter.WriteLine($"переукомплектовать героев заново? (да/нет)");
                            var userDecision = ExpressionWorker.GetUserYesNoDecision(UserInput);

                            if(userDecision == UserDecision.N)
                            {
                                StatusReporter.WriteLine($"игра завершена");
                                HeroUnits.Clear();
                                return;
                            }

                            if(userDecision == UserDecision.Y)
                            {
                                break;
                            }
                            
                        } while (true);
                    }

                    if (generationDone == false) break;
                }

            } while (generationDone == false);

            HeroUnits.InitHeroParameters();
            HeroUnits.FillGroups(GameSettings.nMinPerGroup, GameSettings.nMaxPerGroup);

            StatusReporter.WriteLine("Для битвы доступны следующие замки с укомплектованными героями.");

            foreach (var castle in Castles.OrderBy(castle => castle.Name))
            {
                HeroUnits.ByCastle(castle).ShowNamesGroup(StatusReporter);
            }

            do
            {
                StatusReporter.Write($"Выберите замок ({Castles.Select(castle => castle.ToString()).Aggregate((c, v) => $"{c}/{v}")}) : ");
                var userCastleName = ExpressionWorker.GetUserCastleDecision(UserInput);
                var userCastle = Castles.Find(castle => castle.Equals(new Castle(userCastleName)));
                if (userCastle != null) {
                    StatusReporter.WriteLine($"В Вашем расположении будут воевать герои замка {userCastle}");
                    userCastle.Owner = CastleOwner.User;
                    break;
                }

                StatusReporter.WriteLine($"выбор не определён");

            } while (true);

            DoBattleAction();
        }

        public void DoBattleAction()
        {
            StatusReporter.WriteLine($"действие запущено");
            GameAction = new BattleAction(Castles, HeroUnits);
            GameAction.Scene(UserInput, StatusReporter);
            StatusReporter.WriteLine($"действие завершено");
        }
    }
}
