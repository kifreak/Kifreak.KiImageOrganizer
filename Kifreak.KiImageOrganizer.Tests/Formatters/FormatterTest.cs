using Kifreak.KiImageOrganizer.Console.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kifreak.KiImageOrganizer.Tests.Formatters
{
    [TestClass]
    public class FormatterTest
    {
        [TestMethod]
        public void FileFormatterOk()
        {
            FileFormatters formatters = new FileFormatters();
            string result = formatters.Format("key", "value");
            Assert.AreEqual("key_value", result);
        }

        [TestMethod]
        public void FileFormatterKo()
        {
            FileFormatters formatters = new FileFormatters();
            string result = formatters.Format("key", null);
            Assert.AreEqual("key", result);
            result = formatters.Format(null, "value");
            Assert.AreEqual("_value", result);
            result = formatters.Format(null, null);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void FolderFormatterOk()
        {
            FolderFormatters formatters = new FolderFormatters();
            string result = formatters.Format("key", "value");
            Assert.AreEqual(@"key\value", result);
        }

        [TestMethod]
        public void FolderFormatterKo()
        {
            FolderFormatters formatters = new FolderFormatters();
            string result = formatters.Format("key", null);
            Assert.AreEqual("key", result);
            result = formatters.Format(null, "value");
            Assert.AreEqual(@"\value", result);
            result = formatters.Format(null, null);
            Assert.AreEqual(@"\", result);
        }
    }
}