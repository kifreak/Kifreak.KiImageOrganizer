using Kifreak.KiImageOrganizer.CoreConsole.Configuration;

namespace Kifreak.KiImageOrganizer.CoreConsole.Formatters
{
    public class FolderFormatters : IFormatter
    {
        public string Format(string key, string value)
        {
            return string.IsNullOrEmpty(value) ? key ?? Config.getDefaultPathSlash() : $@"{key}{Config.getDefaultPathSlash()}{value}";
        }
    }
}