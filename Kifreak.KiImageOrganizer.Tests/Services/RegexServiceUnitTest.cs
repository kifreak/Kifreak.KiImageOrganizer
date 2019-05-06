using Kifreak.KiImageOrganizer.Console.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kifreak.KiImageOrganizer.Tests.Services
{
    [TestClass]
    public class RegexServiceUnitTest
    {
        private readonly RegexService _regexService;

        public RegexServiceUnitTest()
        {
            _regexService = new RegexService();
        }

        [TestMethod]
        public void RegexDateTimeFromFileTestOk()
        {
            string result = _regexService.GetDateFromFileName("IMG-20180505-WA0016.png");
            Assert.AreEqual("2018:05:05 00:00:00", result);
        }

        [TestMethod]
        public void ParseDateWithTwoPossibleDates()
        {
            string result = _regexService.GetDateFromFileName("IMG-12345678-20180505.png");
            Assert.AreEqual("2018:05:05 00:00:00", result);
        }

        [TestMethod]
        public void ParseDateWithNoDateInfo()
        {
            string result = _regexService.GetDateFromFileName("IMG-01234-23454.png");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RemoveNotAllowedCharactersTest()
        {
            string testString = "<>:“\"/\\|?*Hi";
            var newString = _regexService.RemoveAllNotAllowedCharactersInAPath(testString);
            Assert.AreEqual("Hi", newString);
        }
    }
}