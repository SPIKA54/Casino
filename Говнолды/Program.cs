using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Говнолды;

namespace Lesson_11
{
    internal class Program
    {
        static IFirebaseClient client;

        static void PrintTablePlayers(Dictionary<string, Players> data)
        {
            Console.WriteLine("Name\t|Age\t|WinsCount\t|");
            foreach (var item in data.Values)
                Console.WriteLine($"{item.Name}\t|{item.Age}\t|{item.WinsCount}\t|");
        }

        static void PrintTableGames(Dictionary<string, Games> data)
        {
            Console.WriteLine("Name\t|Income\t|WinsCount\t|");
            foreach (var item in data.Values)
                Console.WriteLine($"{item.Name}\t|{item.Income}\t|{item.WinsCount}\t|");
        }

        static void PrintTableBudget(Dictionary<string, Budget> data)
        {
            Console.WriteLine("Income\t|service\t|salary\t|");
            foreach (var item in data.Values)
                Console.WriteLine($"{item.Income}\t|{item.service}\t|{item.salary}\t|");
        }

        static Dictionary<string, T> Load<T>(string path)
        {
            FirebaseResponse res = client.Get(path);
            return JsonConvert.DeserializeObject<Dictionary<string, T>>(res.Body)
                   ?? new Dictionary<string, T>();
        }

        static int ReadInt()
        {
            int value;
            while (!int.TryParse(Console.ReadLine(), out value))
                Console.WriteLine("Введіть число:");
            return value;
        }

        static void Main(string[] args)
        {
            IFirebaseConfig ifc = new FirebaseConfig()
            {
                AuthSecret = "<>",
                BasePath = "https://casino-3e423-default-rtdb.europe-west1.firebasedatabase.app"
            };
            client = new FirebaseClient(ifc);

            while (true)
            {
                Console.WriteLine("0.Вихід\n1.Вивід даних\n2.Ввід даних");
                int task = ReadInt();
                if (task == 0) break;

                Console.WriteLine("1.Гравці, 2.Ігри, 3.Бюджет");
                int subtask = ReadInt();

                if (task == 1)
                {
                    switch (subtask)
                    {
                        case 1: PrintTablePlayers(Load<Players>("Players/")); break;
                        case 2: PrintTableGames(Load<Games>("Games/")); break;
                        case 3: PrintTableBudget(Load<Budget>("Budget/")); break;
                    }
                }
                else if (task == 2)
                {
                    switch (subtask)
                    {
                        case 1:
                            Console.WriteLine("Введіть через ентер ім'я гравця, вік та кількість перемог");
                            var player = new Players
                            {
                                Name = Console.ReadLine(),
                                Age = ReadInt(),
                                WinsCount = ReadInt()
                            };
                            client.Set("Players/" + player.Name, player);
                            break;
                        case 2:
                            Console.WriteLine("Введіть через ентер назву автомату, дохід та кількість перемог");
                            var game = new Games
                            {
                                Name = Console.ReadLine(),
                                Income = ReadInt(),
                                WinsCount = ReadInt()
                            };
                            client.Set("Games/" + game.Name, game);
                            break;
                        case 3:
                            Console.WriteLine("Введіть через ентер дохід, витрати на сервіс та зарплату");
                            var budget = new Budget
                            {
                                Income = ReadInt(),
                                service = ReadInt(),
                                salary = ReadInt()
                            };
                            client.Push("Budget/", budget);
                            break;
                    }
                }
            }
        }
    }
}