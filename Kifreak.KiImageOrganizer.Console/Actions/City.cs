using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;
using Kifreak.KiImageOrganizer.Console.Services;


namespace Kifreak.KiImageOrganizer.Console.Actions
{
   
    public class City : SubFolderDecorator
    {
        private readonly ActionModel _model;
        private readonly GeoService _geoService;
        private const string _noLocationString = "NoLocation";

        public City(ActionModel model) : base(model.Folders)
        {
            _model = model;
            _geoService = Config.Get<GeoService>(new[] {"path", "handler"},
                new object[] {Directory.GetCurrentDirectory(), new HttpClientHandler()});
        }
        public override async Task<string> GetSubFolder(IFormatter formatter)
        {
            return formatter.Format(await _subFolders.GetSubFolder(formatter), await GetSubFolderFromAction(formatter));
        }

        private async Task<string> GetSubFolderFromAction(IFormatter formatter)
        {
            Coordinates coordinates = GetCoordinates();
            if (coordinates == null || !coordinates.IsValid()) return _noLocationString;

            OSMData osmData =await _geoService.GetOsmData(coordinates);
            return GetValue(osmData) ?? (_model.Alternative != null ? await _model.Alternative.GetSubFolder(formatter) : _noLocationString);
        }

        private string GetValue(OSMData osmData)
        {
            PropertyInfo property = GetProperty(osmData);
            if (property == null) return _noLocationString;
            object value = property.GetValue(osmData.address);
            return value?.ToString();
        }

        private PropertyInfo GetProperty(OSMData osmData)
        {
            var osmType = osmData.address.GetType();
            return osmType.GetProperty(_model.Type.Type);
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
            string coordinate = _model.MetadataService.GetKey(key);
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