using System;

namespace ZADANIE_6
{
    class ShadowMage
    {
        private int health;
        private readonly int maxHealth = 100;
        private readonly Random random = new Random();
        private int bossFireTurnsLeft = 0;

        public ShadowMage()
        {
            health = maxHealth;
        }

        public void Attack(Boss boss)
        {
            string spell = ChooseSpell();
            switch (spell)
            {
                case "Благословение":
                    UseBlessing();
                    break;
                case "Солнечный луч":
                    UseSunbeam(boss);
                    break;
                case "Рассвет":
                    UseDawn(boss);
                    break;
                case "Небесный огонь":
                    UseHeavenlyFire(boss);
                    break;
                case "Ослепление":
                    UseBlind(boss);
                    break;
            }
        }

        private string ChooseSpell()
        {
            Console.WriteLine("Выберите заклинание:");
            Console.WriteLine("1. Благословение"); // Хилл от 5 до 20.
            Console.WriteLine("2. Солнечный луч"); // Урон от 6 до 30
            Console.WriteLine("3. Рассвет"); // Ослабление босса(урон) на 3 хода
            Console.WriteLine("4. Небесный огонь"); // Урон от 6 до 20 + горение на 3 хода
            Console.WriteLine("5. Ослепление"); // Пропуск хода босса
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 5)
                {
                    break;
                }
                Console.WriteLine("Неверный ввод. Попробуйте снова.");
            }

            Console.WriteLine();
            switch (choice)
            {
                case 1:
                    return "Благословение";
                case 2:
                    return "Солнечный луч";
                case 3:
                    return "Рассвет";
                case 4:
                    return "Небесный огонь";
                case 5:
                    return "Ослепление";
                default:
                    return "";
            }

        }

        private void UseBlessing()
        {
            int heal = random.Next(5, 21); // Хилл от 5 до 20 
            health += heal;
            Console.WriteLine($"Вы наложили благословение на себя. " +
                $"Вы похилили себя на {heal} оз. " +
                $"Ваше текущее здоровье: {health}");
        }

        private void UseSunbeam(Boss boss)
        {
            int damage = random.Next(6, 31); // Урон от 6 до 30
            Console.WriteLine($"Вы использовали Солнечный луч.");
            boss.TakeDamage(damage);
        }

        private void UseDawn(Boss boss)
        {
            Console.WriteLine($"Вы использовали Рассвет. Урон босса уменьшен на 3 хода.");
            boss.ReduceDamage();
        }

        private void UseHeavenlyFire(Boss boss)
        {
            int damage = random.Next(6, 21); // Урон от 6 до 20
            Console.WriteLine($"Вы использовали Небесный огонь.");
            boss.TakeDamage(damage);
            bossFireTurnsLeft = 3;
        }

        private void UseBlind(Boss boss)
        {
            Console.WriteLine("Вы использовали Ослепление.");
            boss.Blind();
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            Console.WriteLine($"Босс нанес вам {damage} урона. " +
                $"Ваше текущее здоровье: {health}");
        }

        public void HandleEffects(Boss boss)
        {

            if (bossFireTurnsLeft > 0)
            {
                int fireDamage = random.Next(0, 7); // Урон от 1 до 6
                Console.WriteLine($"Босс горит и получает {fireDamage} урона.");
                boss.TakeDamage(fireDamage);
                bossFireTurnsLeft--;
            }
        }

        public bool IsAlive()
        {
            return health > 0;
        }
    }

    class Boss
    {
        private int health;
        private readonly int maxHealth = 200;
        private readonly Random random = new Random();
        private int blindTurnsLeft = 0;
        private int reduceDamage = 0;

        public Boss()
        {
            health = maxHealth;
        }

        public void Attack(ShadowMage mage)
        {
            if (blindTurnsLeft > 0)
            {
                Console.WriteLine("Босс ослеплен и пропускает ход.");
                blindTurnsLeft--;
                return;
            }

            int damage = random.Next(10, 25); // Урон от 10 до 25
            if (reduceDamage > 0)
            {
                int damageReduction = random.Next(1, 7); // Уменьшение урона от 1 до 6
                Console.WriteLine($"Урон босса был: {damage} и уменьшен " +
                    $"на {damageReduction}.");
                damage -= damageReduction;
                reduceDamage--;
            }
            mage.TakeDamage(damage);
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            Console.WriteLine($"Вы нанесли боссу {damage} урона. Здоровье босса: {health}");
        }

        public bool IsAlive()
        {
            return health > 0;
        }

        public void ReduceDamage()
        {
            reduceDamage = 3;
        }

        public void Blind()
        {
            blindTurnsLeft = 1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ShadowMage mage = new ShadowMage();
            Boss boss = new Boss();
            bool mageTurn = new Random().Next(0, 2) == 0;

            Console.WriteLine("Бой начинается!" +
                Environment.NewLine + 
                "|------------------------|" +
                Environment.NewLine);

            while (mage.IsAlive() && boss.IsAlive())
            {
                if (mageTurn)
                {
                    mage.Attack(boss);
                    mage.HandleEffects(boss);
                }
                else
                {
                    boss.Attack(mage);
                }

                mageTurn = !mageTurn;
                Console.WriteLine();
            }

            if (mage.IsAlive())
            {
                Console.WriteLine("Вы победили босса!");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Босс победил вас...");
                Console.ReadLine();
            }
        }
    }
}
