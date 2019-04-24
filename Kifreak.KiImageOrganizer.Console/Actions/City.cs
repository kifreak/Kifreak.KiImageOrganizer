﻿using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;
using Kifreak.KiImageOrganizer.Console.Services;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Helpers;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class City : SubFolderDecorator
    {
        private readonly ActionModel _model;
        private readonly IGeoService _geoService;
        private const string _noLocationString = "NoLocation";

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
            if (coordinates == null || !coordinates.IsValid()) return _noLocationString;

            OSMData osmData = await _geoService.GetOsmData(coordinates);
            var result = GetValue(osmData);
            return result ?? await ActionHelpers.ExecuteWithAlternative(_model.Alternative, _noLocationString);
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