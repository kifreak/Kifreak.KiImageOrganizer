namespace Kifreak.KiImageOrganizer.CoreConsole.Models
{
    public class Coordinates
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        
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
            return Latitude + "," + Longitude;
        }
    }
}