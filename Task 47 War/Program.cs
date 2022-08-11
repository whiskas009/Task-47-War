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

        public Soldier(string flagCountry, int health, int attack, int armor, string type = "Обычный")
        {
            FlagCountry = flagCountry;
            Health = health;
            Attack = attack;
            Armor = armor;
            Type = type;
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
    }

    class Regenerate: Soldier
    {
        public Regenerate(string name) : base(name, 100, 20, 15, "Регенерирющий") { }

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
        public Clever(string name) : base(name, 70, 20, 0, "Ловкий") { }

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
        public Mutant(string name) : base(name, 100, 20, 0, "Мутант") 
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
        public string FlagCountry { get; private set; }
        
        private List<Soldier> _soldiers = new List<Soldier>();

        public Platoon(Random random, string flagCountry)
        {
            FlagCountry = flagCountry;
            RecruitSoldiers(random, flagCountry);
        }

        public void ShowInfoSoldier()
        {
            for (int i = 0; i < _soldiers.Count; i++)
            {
                _soldiers[i].ShowInfo();
            }
        }

        public int GetNumberSolders()
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

    class Battlefield
    {
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
            int firstPlatoon = 0;
            int secondPlatoon = 1;

            while (_platoons[firstPlatoon].GetNumberSolders() > 0 && _platoons[secondPlatoon].GetNumberSolders() > 0)
            {
                MakeFightTwoSoldiers();
            }

            if (_platoons[firstPlatoon].GetNumberSolders() > _platoons[secondPlatoon].GetNumberSolders())
            {
                Console.WriteLine($"\nПобедил взвод страны {_platoons[firstPlatoon].FlagCountry}\n");
            }
            else
            {
                Console.WriteLine($"\nПобедил взвод страны {_platoons[secondPlatoon].FlagCountry}\n");
            }
        }

        private void MakeFightTwoSoldiers()
        {
            int minLimit = 0;
            int firstPlatoon = 0;
            int secondPlatoon = 1;
            int firstSoldier = _random.Next(minLimit, _platoons[firstPlatoon].GetNumberSolders());
            int secondSoldier = _random.Next(minLimit, _platoons[secondPlatoon].GetNumberSolders());

            while (_platoons[firstPlatoon].GetHeathSoldier(firstSoldier) > 0 && _platoons[secondPlatoon].GetHeathSoldier(secondSoldier) > 0)
            {
                _platoons[firstPlatoon].TakeDamageSoldier(firstSoldier, _platoons[secondPlatoon].ReturnDamageSoldier(secondSoldier));
                _platoons[secondPlatoon].TakeDamageSoldier(secondSoldier, _platoons[firstPlatoon].ReturnDamageSoldier(firstSoldier));
            }

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
            Console.WriteLine($"Взвод 1. Страна: {_platoons[0].FlagCountry}");
            _platoons[0].ShowInfoSoldier();
            Console.WriteLine($"Взвод 2. Страна: {_platoons[1].FlagCountry}");
            _platoons[1].ShowInfoSoldier();
        }
    }
}
