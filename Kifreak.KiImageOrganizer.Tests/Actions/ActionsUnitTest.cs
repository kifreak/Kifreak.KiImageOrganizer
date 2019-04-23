using System.IO;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kifreak.KiImageOrganizer.Tests.Actions
{
    [TestClass]
    public class ActionsUnitTest
    {
        [TestMethod]
        public async Task MainFolderTest()
        {
            MainFolder mainFolder = new MainFolder(Directory.GetCurrentDirectory());
            string path = await mainFolder.GetSubFolder(new FileFormatters());
            Assert.AreEqual(Directory.GetCurrentDirectory(), path);
        }
        //TODO PENDING MORE TESTS
    }
}