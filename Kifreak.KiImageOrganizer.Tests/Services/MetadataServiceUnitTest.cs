using System.IO;
using Kifreak.KiImageOrganizer.Console.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kifreak.KiImageOrganizer.Tests.Services
{
    [TestClass]
    public class MetadataServiceUnitTest
    {

        [TestMethod]
        public void GetMetadata()
        {
            IMetadataService metadataService = GetMetadataService();
            Assert.IsNotNull(metadataService.GetKey("GPS Latitude"));
        }

        [TestMethod]
        public void GetInvalidMetadata()
        {
            IMetadataService metadataService = GetMetadataService();
            Assert.AreEqual(string.Empty, metadataService.GetKey("No Exist"));
        }

        private IMetadataService GetMetadataService()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            return new MetadataService($@"{currentDirectory}\Resources\Image.jpg");
        }
    }
}