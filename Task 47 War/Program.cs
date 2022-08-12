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
        public Clever(Random random, string name) : base(random, name, "Ловкий") { }

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

        public void DeleteSoldier(int index)
        {
            _soldiers.RemoveAt(index);
        }

        public int GetHeathSoldier(int index)
        {
            return _soldiers[index].Health;
        }

        public void TakeDamageSoldier(int index, int damage)
        {
            _soldiers[index].TakeDamage(damage);
        }

        public int ReturnDamageSoldier(int index)
        {
            return _soldiers[index].ReturnDamage();
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
        private int _firstPlatoon = 0;
        private int _secondPlatoon = 1;
        private List<Platoon> _platoons = new List<Platoon>();
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
            while (_platoons[_firstPlatoon].GetNumberSoldiers() > 0 && _platoons[_secondPlatoon].GetNumberSoldiers() > 0)
            {
                MakeFightTwoSoldiers();
            }

            if (_platoons[_firstPlatoon].GetNumberSoldiers() > _platoons[_secondPlatoon].GetNumberSoldiers())
            {
                Console.WriteLine($"\nПобедил взвод страны {_platoons[_firstPlatoon].FlagCountry}\n");
            }
            else
            {
                Console.WriteLine($"\nПобедил взвод страны {_platoons[_secondPlatoon].FlagCountry}\n");
            }
        }

        private void MakeFightTwoSoldiers()
        {
            int minLimit = 0;
            int firstSoldier = _random.Next(minLimit, _platoons[_firstPlatoon].GetNumberSoldiers());
            int secondSoldier = _random.Next(minLimit, _platoons[_secondPlatoon].GetNumberSoldiers());

            while (_platoons[_firstPlatoon].GetHeathSoldier(firstSoldier) > 0 && _platoons[_secondPlatoon].GetHeathSoldier(secondSoldier) > 0)
            {
                _platoons[_firstPlatoon].TakeDamageSoldier(firstSoldier, _platoons[_secondPlatoon].ReturnDamageSoldier(secondSoldier));
                _platoons[_secondPlatoon].TakeDamageSoldier(secondSoldier, _platoons[_firstPlatoon].ReturnDamageSoldier(firstSoldier));
            }

            AnnounceWinner(_firstPlatoon, _secondPlatoon, firstSoldier, secondSoldier);
        }

        private void CreatePlatoons()
        {
            string nameFirstCountry = "Красная";
            string nameSecondCountry = "Синяя";

            _platoons.Add(new Platoon(_random, nameFirstCountry));
            _platoons.Add(new Platoon(_random, nameSecondCountry));

            Console.WriteLine("\nВзводы созданы\n");
        }

        private void OutputPlatoonsInformation()
        {
            CreatePlatoons();
            Console.WriteLine($"Взвод 1. Страна: {_platoons[_firstPlatoon].FlagCountry}");
            _platoons[_firstPlatoon].ShowInfoSoldiers();
            Console.WriteLine($"Взвод 2. Страна: {_platoons[_secondPlatoon].FlagCountry}");
            _platoons[_secondPlatoon].ShowInfoSoldiers();
        }

        private void AnnounceWinner(int firstPlatoon, int secondPlatoon, int firstSoldier, int secondSoldier)
        {
            if (_platoons[firstPlatoon].GetHeathSoldier(firstSoldier) > _platoons[secondPlatoon].GetHeathSoldier(secondSoldier))
            {
                _platoons[secondPlatoon].DeleteSoldier(secondSoldier);
                Console.WriteLine($"Солдат из страны {_platoons[firstPlatoon].FlagCountry} победил солдата из страны {_platoons[secondPlatoon].FlagCountry}");
            }
            else
            {
                _platoons[firstPlatoon].DeleteSoldier(firstSoldier);
                Console.WriteLine($"Солдат из страны {_platoons[secondPlatoon].FlagCountry} победил солдата из страны {_platoons[firstPlatoon].FlagCountry}");
            }
        }
    }
}
