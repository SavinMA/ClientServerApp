using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = string.Empty;
            int N = 0;

            do
            {
                Console.WriteLine("Введите максимальное количество одновременно обрабатываемых запросов:");
                input = Console.ReadLine();

                if (!int.TryParse(input, out N))
                {
                    Console.WriteLine($"Ошибка. Не удалось привести значение '{input}' к валидному");
                }
            } 
            while (!(N > 0 && N < int.MaxValue));

            Console.WriteLine($"Максимальное количество одновременно обрабатываемых запросов: {N}");

            ClientServer server = new ClientServer(N);
            Task.Run(server.StartListen);

            Console.WriteLine($"Для остановки сервера нажмите любую клавишу.");
            Console.ReadLine();

            server.Stop();
            server.Dispose();
        }
    }
}
