namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class FileFormatters : IFormatter
    {
        public string Format(string key, string value)
        {
            return string.IsNullOrEmpty(value) ? key : $"{key}_{value}";
        }
    }
}