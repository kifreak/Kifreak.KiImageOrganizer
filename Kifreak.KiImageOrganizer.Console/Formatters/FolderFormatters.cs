namespace Kifreak.KiImageOrganizer.Console.Formatters
{
    public class FolderFormatters: IFormatter
    {
        public string Format(string key, string value)
        {
            return string.IsNullOrEmpty(value) ? key : $@"{key}\{value}";
        }
    }
}