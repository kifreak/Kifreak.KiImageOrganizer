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


        public City(string type, SubFolders subFolders, List<KeyValuePair<string, Property>> metadata) : base(
            subFolders, metadata)
        {
            _type = type;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-UK");
            _geoService = new GeoService();
        }

        public override string GetSubFolder()
        {
            return $@"{_subFolders.GetSubFolder()}\{GetSubFolderFromAction()}";
        }

        //TODO: Many IFS - Rewrite.
        private string GetSubFolderFromAction()
        {
            string[] coordinates = GetCoordinates();
            if (coordinates.Length == 0) return "NoLocation";

            OSMData osmData = _geoService.CallOpenStreetMap(coordinates);
            string propertyToSearch = _type;
            if (osmData == null || osmData.address.GetType().GetProperty(propertyToSearch) == null) return "NoLocation";

            object value = osmData.address.GetType().GetProperty(propertyToSearch)?.GetValue(osmData.address);
            if (value == null)
            {
                value = osmData.address.GetType().GetProperty(propertyToSearch)?.GetValue(osmData.address);
                if (value == null) return "NoLocation";
            }

            return value.ToString();
        }

        //TODO Same code por latitude and longitude: Rewrite.
        private string[] GetCoordinates()
        {
            KeyValuePair<string, Property> latitude =
                _metadata.FirstOrDefault(t => t.Key.ToLower() == "latitude" || t.Key.ToLower() == "latitud");
            KeyValuePair<string, Property> longitude =
                _metadata.FirstOrDefault(t => t.Key.ToLower() == "longitude" || t.Key.ToLower() == "longitud");
            if (latitude.Value == null || longitude.Value == null) return new string[0];

            List<double> latDoublesList = ((IEnumerable<double>) latitude.Value.Value).ToList();
            List<double> longDoublesList = ((IEnumerable<double>) longitude.Value.Value).ToList();
            return new[]
            {
                _geoService.ConvertCoordinates(latDoublesList[0], latDoublesList[1], latDoublesList[2]).ToString(CultureInfo.InvariantCulture),
                (-1 * _geoService.ConvertCoordinates(longDoublesList[0], longDoublesList[1], longDoublesList[2]))
                .ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}