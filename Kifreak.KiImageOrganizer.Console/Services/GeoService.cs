using System;
using System.Net;
using System.Threading;
using Kifreak.KiImageOrganizer.Console.Models;
using Newtonsoft.Json;

namespace Kifreak.KiImageOrganizer.Console.Services
{
    public class GeoService
    {
        public double ConvertCoordinates(double degrees, double minutes, double seconds)
        {
            return degrees + (minutes / 60) + (seconds / 3600);
        }
        
        public OSMData CallOpenStreetMap(Coordinates coordinates)
        {
            if (!coordinates.IsValid())
            {
                return new OSMData();
            }
            string url =
                $"https://nominatim.openstreetmap.org/reverse?format=json&lat={coordinates.Latitude}&lon={coordinates.Longitude}";
            var client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                             "Windows NT 5.2; .NET CLR 1.0.3705;)");
            WaitForNextAllowCallToOsm();
            string response = client.DownloadString(url);
            OSMData data = JsonConvert.DeserializeObject<OSMData>(response);
            Config.LastCallToOSM = DateTime.Now;
            return data;
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