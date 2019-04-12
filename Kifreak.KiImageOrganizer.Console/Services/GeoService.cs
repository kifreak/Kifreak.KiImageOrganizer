using System;
using System.IO;
using System.Net;
using System.Threading;
using Kifreak.KiImageOrganizer.Console.Models;
using Newtonsoft.Json;

namespace Kifreak.KiImageOrganizer.Console.Services
{
    public class GeoService
    {
        private readonly string _path;

        public GeoService(string path)
        {
            _path = path ;
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
        public OSMData GetOSMData(Coordinates coordinates)
        {
            if (!coordinates.IsValid())
            {
                return new OSMData();
            }

            OSMData data = ReadFromFile(coordinates);
            if (data != null)
            {
                return data;
            }

            data = CallOSMData(coordinates);
            SaveToFile(coordinates,data);
            return data;
        }

        private OSMData CallOSMData(Coordinates coordinates)
        {
            string url =
                $"https://nominatim.openstreetmap.org/reverse?format=json&lat={coordinates.Latitude}&lon={coordinates.Longitude}";
            var client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                             "Windows NT 5.2; .NET CLR 1.0.3705;)");
            WaitForNextAllowCallToOsm();
            string response = client.DownloadString(url);
            OSMData data = ToOSMData(response);
            Config.LastCallToOSM = DateTime.Now;
            return data;
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

            StreamWriter writer = new StreamWriter(filePath,false);
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