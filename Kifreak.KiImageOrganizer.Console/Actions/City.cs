using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Kifreak.KiImageOrganizer.Console.Models;
using Kifreak.KiImageOrganizer.Console.Services;
using MetaDataFileInfo.Classes;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class City : SubFolderDecorator
    {
        private readonly GeoService _geoService;
        private readonly string _type;
        private const string _noLocationString = "NoLocation";

        public City(string type, SubFolders subFolders, List<KeyValuePair<string, Property>> metadata) : base(
            subFolders, metadata)
        {
            _type = type;
            _geoService = new GeoService();
        }

        public override string GetSubFolder()
        {
            return $@"{_subFolders.GetSubFolder()}\{GetSubFolderFromAction()}";
        }

        private string GetSubFolderFromAction()
        {
            Coordinates coordinates = GetCoordinates();
            if (coordinates==null || !coordinates.IsValid()) return _noLocationString;

            OSMData osmData = _geoService.CallOpenStreetMap(coordinates);
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
                GetCoordinateInfo("latitude"),
                GetCoordinateInfo("longitude")
            );
        }

        private string GetCoordinateInfo(string key)
        {
            KeyValuePair<string, Property> coordinate =
                _metadata.FirstOrDefault(t => t.Key.ToLower() == key);
            List<double> latDoublesList = ((IEnumerable<double>)coordinate.Value.Value).ToList();
            return _geoService.ConvertCoordinates(latDoublesList[0], latDoublesList[1], latDoublesList[2])
                .ToString(CultureInfo.InvariantCulture);
        }
    }
}