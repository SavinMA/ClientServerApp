using CreateTextFiles;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using System.Linq;

namespace unit.test
{
    [TestClass]
    public class FileGeneratorTests
    {
        [TestMethod]
        public void GenerateFile()
        {
            FileGenerator.CreateFile("abc");

            Assert.IsNotNull(FileGenerator.FilesPath);
            Assert.IsTrue(Directory.Exists(FileGenerator.FilesPath));
            Assert.IsTrue(Directory.EnumerateFiles(FileGenerator.FilesPath).Any());

            FileGenerator.RemoveFiles();

            Assert.IsTrue(Directory.Exists(FileGenerator.FilesPath));
            Assert.IsFalse(Directory.EnumerateFiles(FileGenerator.FilesPath).Any());
        }

        [TestMethod]
        public void GenerateFiles_CheckCount()
        {
            int count = 100;

            FileGenerator.GenerateFiles(count);

            Assert.IsTrue(Directory.Exists(FileGenerator.FilesPath));
            Assert.IsTrue(Directory.EnumerateFiles(FileGenerator.FilesPath).Count() == count);

            FileGenerator.RemoveFiles();

            Assert.IsTrue(Directory.Exists(FileGenerator.FilesPath));
            Assert.IsFalse(Directory.EnumerateFiles(FileGenerator.FilesPath).Any());
        }
    }
}
