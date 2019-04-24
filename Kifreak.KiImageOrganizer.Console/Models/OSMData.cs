using Newtonsoft.Json;
using System.Collections.Generic;

// ReSharper disable UnusedMember.Global

namespace Kifreak.KiImageOrganizer.Console.Models
{
    public class OSMData
    {
        [JsonProperty("place_id")]
        public int PlaceId { get; set; }

        [JsonProperty("license")]
        public string License { get; set; }

        [JsonProperty("osm_type")]
        public string OsmType { get; set; }

        [JsonProperty("osm_id")]
        public long OsmId { get; set; }

        [JsonProperty("lat")]
        public string Latitude { get; set; }

        [JsonProperty("lon")]
        public string Longitude { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("boundingbox")]
        public List<string> BoundingBox { get; set; }
    }
}