using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_47_War
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
        }
    }

    class Soldier
    {
        public string FlagCountry { get; protected set; }

        public int Health { get; protected set; }

        public int Attack { get; protected set; }

        public int Armor { get; protected set; }

        public Soldier(string flagCountry, int health, int attack, int armor)
        {
            FlagCountry = flagCountry;
            Health = health;
            Attack = attack;
            Armor = armor;
        }

        public virtual void TakeDamage(int damage)
        {
            if (Armor >= damage)
            {
                Armor -= damage;
            }
            else
            {
                Health -= damage - Armor;
                Armor = 0;
            }
        }

        public virtual int ReturnDaname()
        {
            if (Health > 0)
            {
                return Attack;
            }
            else
            {
                return Attack = 0;
            }
        }
    }

    class Regenerate: Soldier
    {
        public Regenerate(string name) : base(name, 100, 20, 15) { }

        public override void TakeDamage(int damage)
        {
            PartiallyRestoreHealth(damage);
            base.TakeDamage(damage);
        }

        private void PartiallyRestoreHealth(int damage)
        {
            int recoveryFactor = 4;
            int restoredHealth = damage / recoveryFactor;
            Health += restoredHealth;
        }
    }

    class Clever : Soldier
    {
        public Clever(string name) : base(name, 70, 20, 0) { }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(DodgeBlow(damage));
        }

        private int DodgeBlow(int damage)
        {
            Random random = new Random();
            int minLimit = 0;
            int maxLimit = 100;
            int percentProbabilityAction = 80;
            int currentDamage = damage;

            if (random.Next(minLimit, maxLimit) <= percentProbabilityAction)
            {
                currentDamage = 0;
            }

            return currentDamage;
        }
    }

    class Mutant : Soldier
    {
        public Mutant(string name) : base(name, 100, 20, 0) 
        {
            IncreaseCharacteristics();
        }

        private void IncreaseCharacteristics()
        {
            Random random = new Random();
            int minLimit = 0;
            int maxLimit = 100;
            int percentProbabilityAction = 80;
            int increaseFactor = 2;

            if (random.Next(minLimit, maxLimit) <= percentProbabilityAction)
            {
                Health *= increaseFactor;
                Attack *= increaseFactor;
            }
        }
    }

    class Platoon
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public Platoon(Random random, string flagCountry)
        {
            RecruitSoldiers(random, flagCountry);
        }

        private void RecruitSoldiers(Random random, string flagCountry)
        {
            int numberSoldier = 10;

            for (int i = 0; i < numberSoldier; i++)
            {
                RandomChooseSoldier(random, flagCountry, RandomAssignCharacteristics(random));
            }
        }

        private void RandomChooseSoldier(Random random, string flagCountry, List<int> randomCharacteristics)
        {
            int minLimit = 0;
            int maxLimit = 8;
            int probabilityRegenerateSoldier = 0;
            int probabilityCleverSoldier = 1;
            int probabilityMutantSoldier = 2;

            int ramdomNumber = random.Next(minLimit, maxLimit);

            if (ramdomNumber == probabilityMutantSoldier)
            {
                _soldiers.Add(new Mutant(flagCountry));
            }
            else if (ramdomNumber == probabilityRegenerateSoldier)
            {
                _soldiers.Add(new Regenerate(flagCountry));
            }
            else if (ramdomNumber == probabilityCleverSoldier)
            {
                _soldiers.Add(new Clever(flagCountry));
            }
            else
            {
                _soldiers.Add(new Soldier(flagCountry, randomCharacteristics[0], randomCharacteristics[1], randomCharacteristics[2]));
            }
        }

        private List<int> RandomAssignCharacteristics(Random random)
        {
            List<int> characteristics = new List<int>();
            int minHealth = 80;
            int maxHealth = 120;
            int minAttack = 20;
            int maxAttack = 40;
            int minArmor = 0;
            int maxArmor = 40;

            characteristics.Add(random.Next(minHealth,maxHealth));
            characteristics.Add(random.Next(minAttack, maxAttack));
            characteristics.Add(random.Next(minArmor, maxArmor));

            return characteristics;
        }
    }
}
