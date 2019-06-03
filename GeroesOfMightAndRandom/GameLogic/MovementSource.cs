using GeroesOfMightAndRandom.Models;
using System;
using System.Linq;
using GeroesOfMightAndRandom.UserInterface;

namespace GeroesOfMightAndRandom.GameLogic
{
    public class Move
    {
        /// <summary>
        /// Указывает, какая группа героев проводит атаку
        /// </summary>
        public Guid From { get; set; }

        /// <summary>
        /// Указывает, на кого обрушится мощь атакующей гильдии
        /// </summary>
        public Guid To { get; set; }
    }

    /// <summary>
    /// Заготовка "думалки"
    /// </summary>
    public abstract class MovementSource : IMovementSource
    {
        public abstract Move Calculate(Units unitsFrom, Units unitsTo);
    }

    public interface IMovementSource
    {
    }

    /// <summary>
    /// Интерфейс думалки пользователя
    /// </summary>
    public class UserMovementSource : MovementSource, IMovementSource
    {
        public UserMovementSource(IUserInput userInput) {
        }

        /// <summary>
        /// Тут пользователь должен как-то ввести то, кем он хочет напасть и на кого
        /// </summary>
        /// <param name="unitsFrom"></param>
        /// <param name="unitsTo"></param>
        /// <returns></returns>
        public override Move Calculate(Units unitsFrom, Units unitsTo)
        {
            
            return new Move();
        }
    }

    public class AiMovementSource : MovementSource, IMovementSource
    {
        public override Move Calculate(Units unitsFrom, Units unitsTo)
        {
            return new Move();
        }
    }

    public class AiGeneticAlgorithmMovementSource : MovementSource, IMovementSource
    {
        public override Move Calculate(Units unitsFrom, Units unitsTo)
        {
            return new Move();
        }
    }

    /// <summary>
    /// AI с мозгом игрока в рулетку
    /// </summary>
    public class AiRandomMovementSource : MovementSource, IMovementSource
    {
        public override Move Calculate(Units unitsFrom, Units unitsTo)
        {
            var groupsFrom = unitsFrom.GroupBy(unit => unit.GroupIndex).Select(unit => unit.Key).ToList();
            var groupsTo = unitsTo.GroupBy(unit => unit.GroupIndex).Select(unit => unit.Key).ToList();
            var randomMind = new Random();

            var move = new Move {
                From = groupsFrom[randomMind.Next(groupsFrom.Count)],
                To = groupsTo[randomMind.Next(groupsTo.Count)]
            };

            return move;
        }
    }
}
