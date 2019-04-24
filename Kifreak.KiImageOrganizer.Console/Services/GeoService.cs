using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Kifreak.KiImageOrganizer.Console.Services
{
    public class GeoService : IGeoService
    {
        private readonly string _path;
        private readonly HttpMessageHandler _handler;

        public GeoService(string path, HttpMessageHandler handler)
        {
            _path = path;
            _handler = handler;
        }

        public double ConvertCoordinates(double degrees, double minutes, double seconds)
        {
            int multiplier = 1;
            if (degrees < 0)
            {
                multiplier = -1;
            }
            return (multiplier) * (Math.Abs(degrees) + (minutes / 60) + (seconds / 3600));
        }

        public async Task<OSMData> GetOsmData(Coordinates coordinates)
        {
            if (!coordinates.IsValid())
            {
                return Config.Get<OSMData>();
            }

            OSMData data = ReadFromFile(coordinates);
            if (data != null)
            {
                return data;
            }

            data = await CallOsmData(coordinates);
            SaveToFile(coordinates, data);
            return data;
        }

        private async Task<OSMData> CallOsmData(Coordinates coordinates)
        {
            string osmDataResponse = await
                CallUrl(
                    $"https://nominatim.openstreetmap.org/reverse?format=json&lat={coordinates.Latitude}&lon={coordinates.Longitude}");
            OSMData data = ToOSMData(osmDataResponse);
            Config.LastCallToOSM = DateTime.Now;
            await GetAmenityInfo(data);
            return data;
        }

        private async Task GetAmenityInfo(OSMData data)
        {
            string response = await CallUrl($"https://api.openstreetmap.org/api/0.6/{data.OsmType}/{data.OsmId}");
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response);
            if (xml.DocumentElement == null)
            {
                return;
            }
            var amenity = xml.DocumentElement.SelectSingleNode("//*[@k='amenity']") ?? xml.DocumentElement.SelectSingleNode("//*[@k='tourism']");
            var amenityName = xml.DocumentElement.SelectSingleNode("//*[@k='name']");
            if (data.Address == null)
            {
                data.Address = new Address();
            }
            data.Address.AmenityType = amenity?.Attributes?["v"]?.Value;
            data.Address.AmenityName = amenityName?.Attributes?["v"]?.Value;

            Config.LastCallToOSM = DateTime.Now;
        }

        private async Task<string> CallUrl(string url)
        {
            var client = new HttpClient(_handler);
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                                           "Windows NT 5.2; .NET CLR 1.0.3705;)");
            WaitForNextAllowCallToOsm();
            HttpResponseMessage response = await client.GetAsync(url);
            StreamReader reader = new StreamReader(await response.Content.ReadAsStreamAsync());
            string osmDataResponse = reader.ReadToEnd();
            reader.Close();
            return osmDataResponse;
        }

        private OSMData ToOSMData(string text)
        {
            return JsonConvert.DeserializeObject<OSMData>(text);
        }

        private OSMData ReadFromFile(Coordinates coordinates)
        {
            var filePath = $@"{_path}\{coordinates}.log";
            if (!File.Exists(filePath))
            {
                return null;
            }
            StreamReader reader = new StreamReader(filePath);
            var text = reader.ReadToEnd();
            reader.Close();
            return ToOSMData(text);
        }

        private void SaveToFile(Coordinates coordinates, OSMData osmData)
        {
            var filePath = $@"{_path}\{coordinates}.log";
            if (File.Exists(filePath))
            {
                return;
            }

            StreamWriter writer = new StreamWriter(filePath, false);
            writer.WriteLine(JsonConvert.SerializeObject(osmData));
            writer.Close();
        }

        private void WaitForNextAllowCallToOsm()
        {
            while ((DateTime.Now - Config.LastCallToOSM).TotalMilliseconds < 1000)
            {
                Thread.Sleep(100);
            }
        }
    }
}