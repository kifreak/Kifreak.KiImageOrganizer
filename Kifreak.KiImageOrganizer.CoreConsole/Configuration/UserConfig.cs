namespace Kifreak.KiImageOrganizer.CoreConsole.Configuration
{
    public class UserConfig
    {
        /// <summary>
        /// Format for extract date info from File Name.
        /// </summary>
        public string DateFromFileNameFormat { get; set; }

        /// <summary>
        /// Default Format for parse DateTime
        /// </summary>
        public string DateTimeToOrganizeImagesFormat { get; set; }

        /// <summary>
        /// Metadata name for Date Time when photo was taken.
        /// </summary>
        public string MetadataDateTimeOriginal { get; set; }

        /// <summary>
        /// Metadata name for Longitude Coordinate
        /// </summary>
        public string MetadataGpsLongitude { get; set; }

        /// <summary>
        /// Metadata name for Latitude Coordinate
        /// </summary>
        public string MetadataGpsLatitude { get; set; }

        /// <summary>
        /// Text in case no date found.
        /// </summary>
        public string ByDateDefaultText { get; set; }

        /// <summary>
        /// Text in case no location found
        /// </summary>
        public string ByCityDefaultText { get; set; }

        /// <summary>
        /// Degree Symbol for convert coordinates
        /// </summary>
        public string DegreeSymbol { get; set; }

        /// <summary>
        /// Minutes Symbol for convert coordinates
        /// </summary>
        public string MinutesSymbol { get; set; }

        /// <summary>
        /// Folder name where images will be copy
        /// </summary>
        ///  /// <summary>
        /// Seconds Symbol for convert coordinates
        /// </summary>
        public string SecondsSymbol { get; set; }

        /// <summary>
        /// Minutes Symbol for convert coordinates
        /// </summary>
        /// public string MinutesSymbol { get; set; }
        public string DefaultOrganizedPath { get; set; }

        /// <summary>
        /// Separator character for file names
        /// </summary>
        public string FileSeparator { get; set; }

        public UserConfig()
        {
            DateFromFileNameFormat = "yyyyMMdd";
            DateTimeToOrganizeImagesFormat = "yyyy:MM:dd HH:mm:ss";
            MetadataDateTimeOriginal = "Date/Time Original";
            MetadataGpsLatitude = "GPS Latitude";
            MetadataGpsLongitude = "GPS Longitude";
            ByDateDefaultText = "NoDate";
            ByCityDefaultText = "NoLocation";
            DegreeSymbol = "° ";
            MinutesSymbol = "' ";
            SecondsSymbol = "\"";
            DefaultOrganizedPath = "Organized";
            FileSeparator = "_";
        }
    }
}