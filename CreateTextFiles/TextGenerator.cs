using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateTextFiles
{
    /// <summary>
    /// Генерация текста
    /// </summary>
    public static class TextGenerator
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Получение случайного символа
        /// </summary>
        /// <returns>случайный символ</returns>
        public static char GetRandomChar()
        {
            return (char)('A' + random.Next(0, 26));
        }

        /// <summary>
        /// Генерация строки заданной длины из случайных символов
        /// </summary>
        /// <param name="length">длина строки</param>
        /// <returns>строка</returns>
        public static string GenerateString(int length)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append(GetRandomChar());
            }
            return builder.ToString();
        }

        /// <summary>
        /// Генерация палиндрома заданной длины
        /// </summary>
        /// <param name="length">длина строки</param>
        /// <returns>палиндром</returns>
        public static string GeneratePalindrom(int length)
        {
            if (length % 2 == 0)
            {
                var leftString = GenerateString(Convert.ToInt32(length * 0.5));
                var rightString = string.Concat(leftString.Reverse());

                return $"{leftString}{rightString}";
            }
            else
            {
                var leftString = GenerateString(Convert.ToInt32(length * 0.5) - 1);
                var rightString = string.Concat(leftString.Reverse());

                return $"{leftString}{GetRandomChar()}{rightString}";
            }
        }

        /// <summary>
        /// Получение случайной строки или палиндрома размером от 10 до 200 символов 
        /// </summary>
        /// <returns>случайная строка или палиндром</returns>
        public static string GetRandomString()
        {
            var length = random.Next(10, 200);
            return random.Next(0, 2) == 1 ? GeneratePalindrom(length) : GenerateString(length);
        }

        /// <summary>
        /// Получение случайной строки или палиндрома заданного размера
        /// </summary>
        /// <returns>случайная строка или палиндром</returns>
        public static string GetRandomString(int length)
        {
            return random.Next(0, 2) == 1 ? GeneratePalindrom(length) : GenerateString(length);
        }
    }
}
