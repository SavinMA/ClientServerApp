using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateTextFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            int rows = 100;
            for (int row = 0; row < rows; row++)
            {
                var @string = TextGenerator.GetRandomString(11);
                Console.WriteLine($"{row}: {@string}");
            }

            FileGenerator.GenerateFiles(100);

            Console.ReadLine();
        }
    }
}
