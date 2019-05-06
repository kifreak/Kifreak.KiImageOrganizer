namespace Kifreak.KiImageOrganizer.Console.Configuration
{
    public class UserConfig
    {
        public string DateFromFileNameFormat { get; set; }
        public string DateTimeToOrganizeImagesFormat { get; set; }

        public UserConfig()
        {
            DateFromFileNameFormat = "yyyyMMdd";
            DateTimeToOrganizeImagesFormat = "yyyy:MM:dd HH:mm:ss";
        }
    }
}