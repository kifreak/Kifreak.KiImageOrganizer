using System;
using System.Globalization;
using System.IO;
using Kifreak.KiImageOrganizer.Console.Models;
using Kifreak.KiImageOrganizer.Console.Services;


namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class City : SubFolderDecorator
    {
        private readonly GeoService _geoService;
        private readonly string _type;
        private const string _noLocationString = "NoLocation";

        public City(string type, SubFolders subFolders, MetadataService metadata) : base(
            subFolders, metadata)
        {
            _type = type;
            _geoService = new GeoService(Directory.GetCurrentDirectory());
        }

        public override string GetSubFolder()
        {
            return $@"{_subFolders.GetSubFolder()}\{GetSubFolderFromAction()}";
        }

        private string GetSubFolderFromAction()
        {
            Coordinates coordinates = GetCoordinates();
            if (coordinates == null || !coordinates.IsValid()) return _noLocationString;

            OSMData osmData = _geoService.GetOSMData(coordinates);
            return GetValue(osmData);
        }

        private string GetValue(OSMData osmData)
        {
            var property = osmData.address.GetType().GetProperty(_type);
            if (property == null) return "NoLocation";
            object value = property.GetValue(osmData.address);
            return value == null ? "NoLocation" : value.ToString();
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
            string coordinate = _metadata.GetKey(key);
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