using CreateTextFiles;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;

namespace unit.test
{
    [TestClass]
    public class TextGeneratorTests
    {
        public int Length { get; private set; } = 100;

        [TestInitialize]
        public void Setup()
        {
            Length = 100;
        }

        /// <summary>
        /// Проверка генерации строки на соответствие длине строки
        /// </summary>
        [TestMethod]
        public void GenerateString_EqualLength()
        {
            var generatedString = TextGenerator.GenerateString(Length);

            Assert.IsTrue(generatedString.Length == Length);

            var palindrom = TextGenerator.GeneratePalindrom(Length);

            Assert.IsTrue(palindrom.Length == Length);
        }

        /// <summary>
        /// Проверка генерации строки на соответствие палиндрому
        /// </summary>
        [TestMethod]
        public void GeneretePolindrom_EqualSides()
        {
            var palindrom = TextGenerator.GeneratePalindrom(Length);
            var centerLength = (int)(Length * 0.5);
            var leftSide = palindrom.Substring(0, centerLength);
            var right = palindrom.Substring(centerLength, Length - centerLength);
            var rightSide = string.Concat(right.Reverse());

            Assert.AreEqual(leftSide, rightSide);
        }
    }
}
