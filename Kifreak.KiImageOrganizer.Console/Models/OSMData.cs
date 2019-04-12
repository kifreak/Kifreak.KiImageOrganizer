using System.Collections.Generic;
// ReSharper disable UnusedMember.Global

namespace Kifreak.KiImageOrganizer.Console.Models
{
    public class OSMData
    {
        public int place_id { get; set; }
        public string licence { get; set; }
        public string osm_type { get; set; }
        public long osm_id { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string display_name { get; set; }
        public Address address { get; set; }
        public List<string> boundingbox { get; set; }

        public string AmenityName { get; set; }
        public string AmenityType { get; set; }
        
    }
}
