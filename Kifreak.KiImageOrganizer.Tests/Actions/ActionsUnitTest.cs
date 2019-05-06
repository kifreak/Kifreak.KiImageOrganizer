using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;
using Kifreak.KiImageOrganizer.Console.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.Tests.Actions
{
    [TestClass]
    public class ActionsUnitTest
    {
        private readonly Mock<IMetadataService> _metadataServiceMock;
        private readonly string _gpsLatitude = "GPS Latitude";
        private readonly string _gpsLongitude = "GPS Longitude";
        private readonly string _dateTimeOriginal = "Date/Time Original";
        private readonly Mock<IGeoService> _geoServiceMock;

        public ActionsUnitTest()
        {
            _metadataServiceMock = new Mock<IMetadataService>();
            _metadataServiceMock.Setup(t => t.GetKey(_gpsLatitude)).Returns("56° 54' 31.7\"");
            _metadataServiceMock.Setup(t => t.GetKey(_gpsLongitude)).Returns("-5° 51' 05.0\"");
            _metadataServiceMock.Setup(t => t.GetKey(_dateTimeOriginal)).Returns("2017:09:03 18:34:42");
            _geoServiceMock = new Mock<IGeoService>();

            _geoServiceMock.Setup(t => t.GetOsmData(It.IsAny<Coordinates>()))
                .Returns(() => Task.FromResult(new OSMData { Address = new Address { City = "Arisaig", AmenityName = "Beach Bar" } }));
            _geoServiceMock.Setup(t => t.ConvertCoordinates(56, 54, 31.7)).Returns(56.908806);
            _geoServiceMock.Setup(t => t.ConvertCoordinates(-5, 51, 05.0)).Returns(-5.851389);
        }

        [TestMethod]
        public async Task MainFolderTest()
        {
            MainFolder mainFolder = new MainFolder(Directory.GetCurrentDirectory());
            string path = await mainFolder.GetSubFolder(new FileFormatters());
            Assert.AreEqual(Directory.GetCurrentDirectory(), path);
        }

        [TestMethod]
        public async Task NoopTest()
        {
            Noop noop = new Noop(new ActionModel
            {
                Folders = new MainFolder(Directory.GetCurrentDirectory())
            });
            string path = await noop.GetSubFolder(new FileFormatters());
            Assert.AreEqual(string.Empty, path);
        }

        [TestMethod]
        public async Task ByDateTestOk()
        {
            ActionModel model = new ActionModel
            {
                MetadataService = _metadataServiceMock.Object,
                Type = new ActionExecutionModel
                {
                    Type = "HH_mm_ss",
                    Class = "ByDate"
                },
                Alternative = null,
                Folders = new MainFolder(Directory.GetCurrentDirectory())
            };
            await TestByDate(model, $@"{Directory.GetCurrentDirectory()}\18_34_42");
            _metadataServiceMock.Verify(t => t.GetKey(_dateTimeOriginal), Times.Once);
        }

        [TestMethod]
        public async Task CityTestWithNoAlternativeOk()
        {
            ActionModel model = new ActionModel
            {
                MetadataService = _metadataServiceMock.Object,
                Type = new ActionExecutionModel
                {
                    Type = "City",
                    Class = "City"
                },
                Alternative = null,
                Folders = new MainFolder(Directory.GetCurrentDirectory())
            };
            await TestCity(model, $@"{Directory.GetCurrentDirectory()}\Arisaig");
            _metadataServiceMock.Verify(t => t.GetKey(_gpsLatitude), Times.Once);
            _metadataServiceMock.Verify(t => t.GetKey(_gpsLongitude), Times.Once);
            _geoServiceMock.Verify(t => t.GetOsmData(It.IsAny<Coordinates>()), Times.Once);
        }

        [TestMethod]
        public async Task CityTestWithAlternativeOk()
        {
            Config.Startup();
            var model = new ActionModel
            {
                Type = new ActionExecutionModel
                {
                    Type = "FailObject",
                    Class = "City"
                },
                Alternative = new City(new ActionModel
                {
                    Type = new ActionExecutionModel
                    {
                        Type = "AmenityName",
                        Class = "City"
                    },
                    Alternative = null,
                    MetadataService = _metadataServiceMock.Object,
                    Folders = new MainFolder(string.Empty)
                }, _geoServiceMock.Object),
                MetadataService = _metadataServiceMock.Object,
                Folders = new MainFolder(Directory.GetCurrentDirectory())
            };
            await TestCity(model, $@"{Directory.GetCurrentDirectory()}\Beach Bar");
            _metadataServiceMock.Verify(t => t.GetKey(_gpsLatitude), Times.Exactly(2));
            _metadataServiceMock.Verify(t => t.GetKey(_gpsLongitude), Times.Exactly(2));
            _geoServiceMock.Verify(t => t.GetOsmData(It.IsAny<Coordinates>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task CityTestWithNoopAlternativeOk()
        {
            Config.Startup();
            var model = new ActionModel
            {
                Type = new ActionExecutionModel
                {
                    Type = "FailObject",
                    Class = "City"
                },
                Alternative = new Noop(new ActionModel
                {
                    Type = new ActionExecutionModel
                    {
                        Type = "Noop",
                        Class = "Noop"
                    },
                    Alternative = null,
                    MetadataService = _metadataServiceMock.Object,
                    Folders = new MainFolder(Directory.GetCurrentDirectory())
                }),
                MetadataService = _metadataServiceMock.Object,
                Folders = new MainFolder(Directory.GetCurrentDirectory())
            };
            await TestCity(model, Directory.GetCurrentDirectory());
            _metadataServiceMock.Verify(t => t.GetKey(_gpsLatitude), Times.Exactly(1));
            _metadataServiceMock.Verify(t => t.GetKey(_gpsLongitude), Times.Exactly(1));
            _geoServiceMock.Verify(t => t.GetOsmData(It.IsAny<Coordinates>()), Times.Exactly(1));
        }

        private async Task TestCity(ActionModel model, string expectedResponse)
        {
            City city = new City(model, _geoServiceMock.Object);
            string result = await city.GetSubFolder(new FolderFormatters());
            //Asserts

            Assert.AreEqual(expectedResponse, result);
        }

        private async Task TestByDate(ActionModel model, string expectedResponse)
        {
            ByDate city = new ByDate(model, new RegexService());
            string result = await city.GetSubFolder(new FolderFormatters());
            //Asserts

            Assert.AreEqual(expectedResponse, result);
        }
    }
}