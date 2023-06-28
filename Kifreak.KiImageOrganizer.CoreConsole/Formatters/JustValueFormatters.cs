namespace Kifreak.KiImageOrganizer.CoreConsole.Formatters
{
    public class JustValueFormatters : IFormatter
    {
        public string Format(string key, string value)
        {
            return value;
        }
    }
}