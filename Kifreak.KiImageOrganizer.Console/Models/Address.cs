// ReSharper disable UnusedMember.Global

using Newtonsoft.Json;

namespace Kifreak.KiImageOrganizer.Console.Models
{
    public class Address
    {
        [JsonProperty("house_number")]
        public string HouseNumber { get; set; }

        [JsonProperty("road")]
        public string Road { get; set; }

        [JsonProperty("suburb")]
        public string SubUrb { get; set; }

        [JsonProperty("village")]
        public string Village { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("postcode")]
        public string PostCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        public string AmenityName { get; set; }
        public string AmenityType { get; set; }
    }
}