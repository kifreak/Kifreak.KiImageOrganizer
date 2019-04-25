namespace Kifreak.KiImageOrganizer.Console.Formatters
{
    public class SeparatedByCommasFormatters : IFormatter
    {
        public string Format(string key, string value)
        {
            var separator = ",";
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                separator = string.Empty;
            }
            return $"{key}{separator}{value}";
        }
    }
}