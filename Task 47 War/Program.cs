using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_47_War
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Battlefield battlefield = new Battlefield();
            battlefield.StartGame();
        }
    }

    class Soldier
    {
        public string FlagCountry { get; protected set; }
        public string Type { get; protected set; }
        public int Health { get; protected set; }
        public int Attack { get; protected set; }
        public int Armor { get; protected set; }

        public Soldier(Random random ,string flagCountry, string type = "Обычный")
        {
            FlagCountry = flagCountry;
            Type = type;
            AssignRandomCharacteristics(random);
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

        public virtual int ReturnDamage()
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

        public void ShowInfo()
        {
            Console.WriteLine($"Здоровье: {Health} | Аттака: {Attack} | Броня: {Attack} | Тип: {Type}");
        }

        private void AssignRandomCharacteristics(Random random)
        {
            int minHealth = 80;
            int maxHealth = 120;
            int minAttack = 20;
            int maxAttack = 40;
            int minArmor = 0;
            int maxArmor = 40;

            Health = random.Next(minHealth, maxHealth);
            Attack = random.Next(minAttack, maxAttack);
            Armor = random.Next(minArmor, maxArmor);
        }
    }

    class Regenerate: Soldier
    {
        public Regenerate(Random random, string name) : base(random, name, "Регенерирющий") 
        {
            Health = 100;
            Attack = 20;
            Armor = 15;
        }

        public override void TakeDamage(int damage)
        {
            RestorePartiallyHealth(damage);
            base.TakeDamage(damage);
        }

        private void RestorePartiallyHealth(int damage)
        {
            int recoveryFactor = 4;
            int restoredHealth = damage / recoveryFactor;
            Health += restoredHealth;
        }
    }

    class Clever : Soldier
    {
        public Clever(Random random, string name) : base(random, name, "Ловкий") 
        {
            Health = 70;
            Attack = 20;
            Armor = 0;
        }

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
        public Mutant(Random random, string name) : base(random, name, "Мутант") 
        {
            Health = 100;
            Attack = 20;
            Armor = 0;
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

        public string FlagCountry { get; private set; }

        public Platoon(Random random, string flagCountry)
        {
            FlagCountry = flagCountry;
            RecruitSoldiers(random, flagCountry);
        }

        public Soldier RetuernRandomSoldier(Random random)
        {
            Soldier soldier;
            int minLimit = 0;
            int index = random.Next(minLimit, _soldiers.Count);
            soldier = _soldiers.ElementAt(index);
            return soldier;
        }

        public void ShowInfoSoldiers()
        {
            for (int i = 0; i < _soldiers.Count; i++)
            {
                _soldiers[i].ShowInfo();
            }
        }

        public int GetNumberSoldiers()
        {
            return _soldiers.Count();
        }

        public void DeleteSoldier(Soldier soldier)
        {
            int index = _soldiers.IndexOf(soldier);
            _soldiers.RemoveAt(index);
        }

        private void RecruitSoldiers(Random random, string flagCountry)
        {
            int numberSoldier = 5;

            for (int i = 0; i < numberSoldier; i++)
            {
                ChooseRandomTypeSoldier(random, flagCountry);
            }
        }

        private void ChooseRandomTypeSoldier(Random random, string flagCountry)
        {
            int minLimit = 0;
            int maxLimit = 8;
            int probabilityRegenerateSoldier = 0;
            int probabilityCleverSoldier = 1;
            int probabilityMutantSoldier = 2;
            int ramdomNumber = random.Next(minLimit, maxLimit);

            if (ramdomNumber == probabilityMutantSoldier)
            {
                _soldiers.Add(new Mutant(random, flagCountry));
            }
            else if (ramdomNumber == probabilityRegenerateSoldier)
            {
                _soldiers.Add(new Regenerate(random, flagCountry));
            }
            else if (ramdomNumber == probabilityCleverSoldier)
            {
                _soldiers.Add(new Clever(random, flagCountry));
            }
            else
            {
                _soldiers.Add(new Soldier(random, flagCountry));
            }
        }
    }

    class Battlefield
    {
        private Platoon _firstPlatoon;
        private Platoon _secondPlatoon;
        private Random _random = new Random();

        public Battlefield()
        {
            CreatePlatoons();
        }

        public void StartGame()
        {
            bool isWork = true;
            
            while (isWork == true)
            {
                Console.WriteLine("1. Показать информацию о взводах стран \n2. Начать бой \n3. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        OutputPlatoonsInformation();
                        break;

                    case "2":
                        StartBattle();
                        break;

                    case "3":
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("\nНеккоректный ввод\n");
                        break;
                }
            }
        }

        public void StartBattle()
        {
            while (_firstPlatoon.GetNumberSoldiers() > 0 && _secondPlatoon.GetNumberSoldiers() > 0)
            {
                MakeFightTwoSoldiers();
            }

            if (_firstPlatoon.GetNumberSoldiers() > _secondPlatoon.GetNumberSoldiers())
            {
                Console.WriteLine($"\nПобедил взвод страны {_firstPlatoon.FlagCountry}\n");
            }
            else
            {
                Console.WriteLine($"\nПобедил взвод страны {_secondPlatoon.FlagCountry}\n");
            }
        }

        private void MakeFightTwoSoldiers()
        {
            Soldier firstSoldier = _firstPlatoon.RetuernRandomSoldier(_random);
            Soldier secondSoldier = _secondPlatoon.RetuernRandomSoldier(_random);

            while (firstSoldier.Health > 0 && secondSoldier.Health > 0)
            {
                firstSoldier.TakeDamage(secondSoldier.ReturnDamage());
                secondSoldier.TakeDamage(firstSoldier.ReturnDamage());
            }

            AnnounceWinner(firstSoldier, secondSoldier);
        }

        private void CreatePlatoons()
        {
            string nameFirstCountry = "Красная";
            string nameSecondCountry = "Синяя";

            _firstPlatoon = new Platoon(_random, nameFirstCountry);
            _secondPlatoon = new Platoon(_random, nameSecondCountry);

            Console.WriteLine("\nВзводы созданы\n");
        }

        private void OutputPlatoonsInformation()
        {
            CreatePlatoons();
            Console.WriteLine($"Взвод 1. Страна: {_firstPlatoon.FlagCountry}");
            _firstPlatoon.ShowInfoSoldiers();
            Console.WriteLine($"Взвод 2. Страна: {_secondPlatoon.FlagCountry}");
            _secondPlatoon.ShowInfoSoldiers();
        }

        private void AnnounceWinner(Soldier firstSoldier, Soldier secondSoldier)
        {
            if (firstSoldier.Health > secondSoldier.Health)
            {
                _secondPlatoon.DeleteSoldier(secondSoldier);
                Console.WriteLine($"Солдат из страны {_firstPlatoon.FlagCountry} победил солдата из страны {_secondPlatoon.FlagCountry}");
            }
            else
            {
                _firstPlatoon.DeleteSoldier(firstSoldier);
                Console.WriteLine($"Солдат из страны {_secondPlatoon.FlagCountry} победил солдата из страны {_firstPlatoon.FlagCountry}");
            }
        }
    }
}
