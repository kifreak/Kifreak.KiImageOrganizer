using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Models;
using Kifreak.KiImageOrganizer.Console.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using Moq.Protected;

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
                                Content = new StringContent(isFirstCall?GetJsonMock():GetXmlMock()),
                            };
                        isFirstCall = false;
                        return httpResponseMessage;
                    }
                )
                .Verifiable();
         GeoService service = new GeoService(Directory.GetCurrentDirectory(), handlerMock.Object);
            var result = await service.GetOsmData(new Coordinates("56.91071025", "-5.84795337054901"));
            File.Delete($@"{Directory.GetCurrentDirectory()}\56.91071025,-5.84795337054901.log");
            handlerMock.Protected().Verify("SendAsync",
                Times.Exactly(2),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
            
        }

        [TestMethod]
        public void ConvertCoordinatesOK()
        {
            GeoService geoService = new GeoService(Directory.GetCurrentDirectory(), new HttpClientHandler());
            var coordinates = geoService.ConvertCoordinates(15, 15, 15);
            Assert.AreEqual(15.2541667, Math.Round(coordinates,7));
        }
        private string GetJsonMock()
        {
            return
                "{\"place_id\":96097496,\"licence\":\"Data OpenStreetMap contributors, ODbL 1.0. https://osm.org/copyright\",\"osm_type\":\"way\",\"osm_id\":94964932,\"lat\":\"56.91071025\",\"lon\":\"-5.84795337054901\",\"display_name\":\"Arisaig Hotel, B8008, Arisaig, Highland, Scotland, PH39 4NP, UK\",\"address\":{\"house_number\":null,\"road\":\"B8008\",\"suburb\":null,\"village\":\"Arisaig\",\"city\":null,\"county\":\"Highland\",\"state\":\"Scotland\",\"postcode\":\"PH39 4NP\",\"country\":\"UK\",\"country_code\":\"gb\",\"AmenityName\":\"Arisaig Hotel\",\"AmenityType\":\"hotel\"},\"boundingbox\":[\"56.9105195\",\"56.9109547\",\"-5.8483457\",\"-5.847677\"]}";
        }

        private string GetXmlMock()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><osm version=\"0.6\" generator=\"CGImap 0.6.1 (11655 thorn-03.openstreetmap.org)\" copyright=\"OpenStreetMap and contributors\" attribution=\"http://www.openstreetmap.org/copyright\" license=\"http://opendatacommons.org/licenses/odbl/1-0/\"><node id=\"2907784231\" visible=\"true\" version=\"2\" changeset=\"22911365\" timestamp=\"2014-06-13T15:55:17Z\" user=\"eisa\" uid=\"1939732\" lat=\"55.9473887\" lon=\"-3.1919036\"><tag k=\"addr:city\" v=\"Edinburgh\"/><tag k=\"addr:country\" v=\"GB\"/><tag k=\"addr:housenumber\" v=\"29\"/><tag k=\"addr:street\" v=\"Candlemaker Row\"/><tag k=\"source\" v=\"survey\"/></node></osm>";
        }
    }

 
}