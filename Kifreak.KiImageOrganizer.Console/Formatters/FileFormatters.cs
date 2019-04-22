namespace Kifreak.KiImageOrganizer.Console.Formatters
{
    public class FileFormatters : IFormatter
    {
        public string Format(string key, string value)
        {
            return string.IsNullOrEmpty(value) ? key??string.Empty : $"{key}_{value}";
        }
    }
}