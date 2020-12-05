using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class ConsoleWriter
    {
        private static object _consoleLock = new object();

        public static void WriteError(string message)
        {
            lock (_consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public static void WriteMessage(string message)
        {
            lock(_consoleLock)
            {
                Console.WriteLine(message);
            }
        }
    }
}
