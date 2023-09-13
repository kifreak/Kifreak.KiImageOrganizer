using Kifreak.KiImageOrganizer.CoreConsole.Configuration;

namespace Kifreak.KiImageOrganizer.CoreConsole.Formatters
{
    public class FolderFormatters : IFormatter
    {
        public string Format(string key, string value)
        {
            return string.IsNullOrEmpty(value) ? key ?? Path.DirectorySeparatorChar.ToString() : Path.Combine(key, value);
        }
    }
}