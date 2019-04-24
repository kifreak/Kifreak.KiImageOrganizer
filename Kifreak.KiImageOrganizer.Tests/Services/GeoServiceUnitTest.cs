using Kifreak.KiImageOrganizer.Console.Models;
using Kifreak.KiImageOrganizer.Console.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.Tests.Services
{
    [TestClass]
    public class GeoServiceUnitTest
    {
        [TestMethod]
        public async Task GetOsmDataTest()
        {
            bool isFirstCall = true;
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(() =>
                    {
                        var httpResponseMessage =
                            new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent(isFirstCall ? GetJsonMock() : GetXmlMock()),
                            };
                        isFirstCall = false;
                        return httpResponseMessage;
                    }
                )
                .Verifiable();
            IGeoService service = new GeoService(Directory.GetCurrentDirectory(), handlerMock.Object);
            await service.GetOsmData(new Coordinates("56.91071025", "-5.84795337054901"));
            File.Delete($@"{Directory.GetCurrentDirectory()}\56.91071025,-5.84795337054901.log");
            handlerMock.Protected().Verify("SendAsync",
                Times.Exactly(2),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public void ConvertCoordinatesOK()
        {
            IGeoService geoService = new GeoService(Directory.GetCurrentDirectory(), new HttpClientHandler());
            var coordinates = geoService.ConvertCoordinates(15, 15, 15);
            Assert.AreEqual(15.2541667, Math.Round(coordinates, 7));
        }

        [TestMethod]
        public void ConvertCoordinatesWithNegativesOK()
        {
            IGeoService geoService = new GeoService(Directory.GetCurrentDirectory(), new HttpClientHandler());
            var coordinates = geoService.ConvertCoordinates(-5, 51, 05.0);
            Assert.AreEqual(-5.851389, Math.Round(coordinates, 6));
            var otherCoordinates = geoService.ConvertCoordinates(-40, 50, 29.0);
            Assert.AreEqual(-40.841389, Math.Round(otherCoordinates, 6));
        }

        [TestMethod]
        public void WrongCoordinatesTest()
        {
            IGeoService geoService = new GeoService(Directory.GetCurrentDirectory(), new HttpClientHandler());
            var coordinates = geoService.ConvertCoordinates(-1000, int.MaxValue, 3);
            var zeroCoordinates = geoService.ConvertCoordinates(0, 0, 0);
            var maxCoordinates = geoService.ConvertCoordinates(int.MaxValue, int.MaxValue, int.MaxValue);
            Assert.AreEqual(-35792394.1175, coordinates);
            Assert.AreEqual(0, zeroCoordinates);
            Assert.IsTrue(maxCoordinates > int.MaxValue);
        }

        private string GetJsonMock()
        {
            return
                "{\"PlaceId\":96097496,\"License\":\"Data OpenStreetMap contributors, ODbL 1.0. https://osm.org/copyright\",\"OsmType\":\"way\",\"OsmId\":94964932,\"Latitude\":\"56.91071025\",\"lon\":\"-5.84795337054901\",\"display_name\":\"Arisaig Hotel, B8008, Arisaig, Highland, Scotland, PH39 4NP, UK\",\"Address\":{\"house_number\":null,\"Road\":\"B8008\",\"suburb\":null,\"Village\":\"Arisaig\",\"City\":null,\"County\":\"Highland\",\"State\":\"Scotland\",\"PostCode\":\"PH39 4NP\",\"Country\":\"UK\",\"CountryCode\":\"gb\",\"AmenityName\":\"Arisaig Hotel\",\"AmenityType\":\"hotel\"},\"boundingbox\":[\"56.9105195\",\"56.9109547\",\"-5.8483457\",\"-5.847677\"]}";
        }

        private string GetXmlMock()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><osm version=\"0.6\" generator=\"CGImap 0.6.1 (11655 thorn-03.openstreetmap.org)\" copyright=\"OpenStreetMap and contributors\" attribution=\"http://www.openstreetmap.org/copyright\" license=\"http://opendatacommons.org/licenses/odbl/1-0/\"><node id=\"2907784231\" visible=\"true\" version=\"2\" changeset=\"22911365\" timestamp=\"2014-06-13T15:55:17Z\" user=\"eisa\" uid=\"1939732\" Latitude=\"55.9473887\" lon=\"-3.1919036\"><tag k=\"addr:City\" v=\"Edinburgh\"/><tag k=\"addr:Country\" v=\"GB\"/><tag k=\"addr:housenumber\" v=\"29\"/><tag k=\"addr:street\" v=\"Candlemaker Row\"/><tag k=\"source\" v=\"survey\"/></node></osm>";
        }
    }
}