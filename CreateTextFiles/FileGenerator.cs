using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateTextFiles
{
    /// <summary>
    /// Генерация файлов
    /// </summary>
    public static class FileGenerator
    {
        /// <summary>
        /// Базовый путь
        /// </summary>
        public static string FilesPath = Path.Combine(AppContext.BaseDirectory, "Files");

        static FileGenerator()
        {
            if (!Directory.Exists(FilesPath))
                Directory.CreateDirectory(FilesPath);
        }


        /// <summary>
        /// Создание файла и запись в него текста
        /// </summary>
        /// <param name="text">Текст</param>
        public static void CreateFile(string text)
        {
            string path;
            do
            {
                var fileName = Guid.NewGuid().ToString() + ".txt";
                path = Path.Combine(FilesPath, fileName);
            } while (File.Exists(path));

            File.WriteAllText(path, text);
        }

        /// <summary>
        /// Удаление файлов из базовой директории
        /// </summary>
        public static void RemoveFiles()
        {
            Directory.EnumerateFiles(FilesPath)
                .AsParallel()
                .ForAll(filePath => File.Delete(filePath));
        }

        /// <summary>
        /// Генерация заданого количества файлов
        /// </summary>
        /// <param name="count">количество файлов</param>
        public static void GenerateFiles(int count)
        {
            RemoveFiles();
            Enumerable.Range(0, count)
                .AsParallel()
                .ForAll((i) =>
                {
                    var text = TextGenerator.GetRandomString();
                    CreateFile(text);
                });
        }
    }
}
