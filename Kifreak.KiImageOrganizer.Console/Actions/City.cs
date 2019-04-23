using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;
using Kifreak.KiImageOrganizer.Console.Services;


namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class City : SubFolderDecorator
    {
        private readonly GeoService _geoService;
        private readonly string _type;
        private const string _noLocationString = "NoLocation";

        public City(string type, SubFolders subFolders) : base(
            subFolders)
        {
            _type = type;
            _geoService = new GeoService(Directory.GetCurrentDirectory(), new HttpClientHandler());
        }

        public override async Task<string> GetSubFolder(IFormatter formatter)
        {
            return formatter.Format(await _subFolders.GetSubFolder(formatter), await GetSubFolderFromAction());
        }

        private async Task<string> GetSubFolderFromAction()
        {
            Coordinates coordinates = GetCoordinates();
            if (coordinates == null || !coordinates.IsValid()) return _noLocationString;

            OSMData osmData =await _geoService.GetOsmData(coordinates);
            return GetValue(osmData);
        }

        private string GetValue(OSMData osmData)
        {
            PropertyInfo property = GetProperty(osmData);
            if (property == null) return _noLocationString;
            object value = property.GetValue(osmData.address);
            return value == null ? _noLocationString : value.ToString();
        }

        private PropertyInfo GetProperty(OSMData osmData)
        {
            var osmType = osmData.address.GetType();
            return osmType.GetProperty(osmType.GetProperty(_type)?.GetValue(osmData.address) != null || string.IsNullOrEmpty(Alternative)? _type : Alternative?.ToLower());
        }
        private Coordinates GetCoordinates()
        {
            return new Coordinates(
                GetCoordinateInfo("GPS Latitude"),
                GetCoordinateInfo("GPS Longitude")
            );
        }

        private string GetCoordinateInfo(string key)
        {
            string coordinate = Metadata.GetKey(key);
            if (string.IsNullOrEmpty(coordinate))
            {
                return string.Empty;
            }

            int degreeSymbol = coordinate.IndexOf("° ", StringComparison.Ordinal);
            int minutesSymbol = coordinate.IndexOf("' ", StringComparison.Ordinal);
            int secondsSymbol = coordinate.IndexOf("\"", StringComparison.Ordinal);
            if (degreeSymbol < 0 || minutesSymbol < 0 || secondsSymbol < 0)
            {
                return string.Empty;
            }

            double.TryParse(coordinate.Substring(0, degreeSymbol), out double degrees);
            double.TryParse(coordinate.Substring(degreeSymbol + 2, minutesSymbol - (degreeSymbol + 2)),
                out double minutes);
            double.TryParse(coordinate.Substring(minutesSymbol + 2, secondsSymbol - (minutesSymbol + 2)),
                out double seconds);
            return _geoService.ConvertCoordinates(degrees, minutes, seconds).ToString(CultureInfo.InvariantCulture);
        }
    }
}