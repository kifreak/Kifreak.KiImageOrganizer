using Kifreak.KiImageOrganizer.CoreConsole.Configuration;

namespace Kifreak.KiImageOrganizer.CoreConsole.Formatters
{
    public class FileFormatters : IFormatter
    {
        public string Format(string key, string value)
        {
            return string.IsNullOrEmpty(value) ? key ?? string.Empty : $"{key}{Config.UserConfig.FileSeparator}{value}";
        }
    }
}