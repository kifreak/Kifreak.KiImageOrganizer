using System;
using Kifreak.KiImageOrganizer.Console.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kifreak.KiImageOrganizer.Tests
{
    [TestClass]
    public class CommandsHelperTest
    {
        [TestMethod]
        public void RemoveNotAllowedCharactersTest()
        {
            string testString = "<>:“\"/\\|?*Hi";
            var newString = CommandsHelper.RemoveAllNotAllowedCharacters(testString);
            Assert.AreEqual( "Hi", newString);
        }
    }
}
