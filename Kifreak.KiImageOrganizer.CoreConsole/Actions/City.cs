using Kifreak.KiImageOrganizer.CoreConsole.Configuration;
using Kifreak.KiImageOrganizer.CoreConsole.Formatters;
using Kifreak.KiImageOrganizer.CoreConsole.Models;
using Kifreak.KiImageOrganizer.CoreConsole.Services;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.CoreConsole.Helpers;

namespace Kifreak.KiImageOrganizer.CoreConsole.Actions
{
    public class City : SubFolderDecorator
    {
        private readonly ActionModel _model;
        private readonly IGeoService _geoService;

        // ReSharper disable once UnusedMember.Global
        public City(ActionModel model) : base(model.Folders)
        {
            _model = model;
            _geoService = Config.Get<IGeoService>(new[] { "path", "handler" },
                new object[] { Directory.GetCurrentDirectory(), new HttpClientHandler() });
        }

        public City(ActionModel model, IGeoService geoService) : base(model.Folders)
        {
            _model = model;
            _geoService = geoService;
        }

        public override async Task<string> GetSubFolder(IFormatter formatter)
        {
            return formatter.Format(await _subFolders.GetSubFolder(formatter), await GetSubFolderFromAction());
        }

        private async Task<string> GetSubFolderFromAction()
        {
            Coordinates coordinates = GetCoordinates();
            string result = null;
            if (coordinates != null && coordinates.IsValid())
            {
                OSMData osmData = await _geoService.GetOsmData(coordinates);
                result = GetValue(osmData);
            }
            return result ?? await ActionHelpers.ExecuteWithAlternative(_model.Alternative, Config.UserConfig.ByCityDefaultText);
        }

        private string GetValue(OSMData osmData)
        {
            PropertyInfo property = GetProperty(osmData);
            if (property == null) return null;
            object value = property.GetValue(osmData.Address);
            return value?.ToString();
        }

        private PropertyInfo GetProperty(OSMData osmData)
        {
            var osmType = osmData.Address.GetType();
            return osmType.GetProperty(_model.Type.Type);
        }

        private Coordinates GetCoordinates()
        {
            return new Coordinates(
                GetCoordinateInfo(Config.UserConfig.MetadataGpsLatitude),
                GetCoordinateInfo(Config.UserConfig.MetadataGpsLongitude)
            );
        }

        private string GetCoordinateInfo(string key)
        {
            string coordinate = _model.MetadataService.GetKey(key);
            if (string.IsNullOrEmpty(coordinate))
            {
                return string.Empty;
            }

            int degreeSymbol = coordinate.IndexOf(Config.UserConfig.DegreeSymbol, StringComparison.Ordinal);
            int minutesSymbol = coordinate.IndexOf(Config.UserConfig.MinutesSymbol, StringComparison.Ordinal);
            int secondsSymbol = coordinate.IndexOf(Config.UserConfig.SecondsSymbol, StringComparison.Ordinal);
            if (degreeSymbol < 0 || minutesSymbol < 0 || secondsSymbol < 0)
            {
                return string.Empty;
            }

            double.TryParse(coordinate.Substring(0, degreeSymbol), out double degrees);
            double.TryParse(coordinate.Substring(degreeSymbol + Config.UserConfig.DegreeSymbol.Length, minutesSymbol - (degreeSymbol + Config.UserConfig.DegreeSymbol.Length)),
                out double minutes);
            double.TryParse(coordinate.Substring(minutesSymbol + Config.UserConfig.MinutesSymbol.Length, secondsSymbol - (minutesSymbol + Config.UserConfig.MinutesSymbol.Length)),
                out double seconds);
            return _geoService.ConvertCoordinates(degrees, minutes, seconds).ToString(CultureInfo.InvariantCulture);
        }
    }
}