using System;
using GeroesOfMightAndRandom.Statics;

namespace GeroesOfMightAndRandom.Models
{
    /// <summary>
    /// Версия для основных параметров героя
    /// </summary>
    [Serializable]
    public class HeroPrototype
    {
        /// <summary>
        /// Имя героя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Урон
        /// </summary>
        public double Damage { get; set; }


        private double _life;

        /// <summary>
        /// Жизнь
        /// </summary>
        public double Life {
            get => _life; 
            set {
                _life = value;
                _life = _life < 0 ? 0 : _life;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="life">Жизнь</param>
        /// <param name="damage">Урон</param>
        /// <param name="damageAccuracy">Погрешность урона</param>
        public HeroPrototype(int life, int damage, int damageAccuracy)
        {
            Damage = damage + Utils.GetDoubleRandom(damageAccuracy);
            Life = life;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
