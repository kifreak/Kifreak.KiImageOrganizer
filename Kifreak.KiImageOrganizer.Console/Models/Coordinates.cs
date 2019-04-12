namespace Kifreak.KiImageOrganizer.Console.Models
{
    public class Coordinates
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public Coordinates(string[] coordinates)
        {
            if (coordinates == null && coordinates.Length == 2)
            {
                return;
            }

            Latitude = coordinates[0];
            Longitude = coordinates[1];
        }

        public Coordinates(string latitude, string longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Latitude) && !string.IsNullOrEmpty(Longitude);
        }

        public override string ToString()
        {
            return Latitude + "-" + Longitude;
        }
    }
}