using System;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class FolderFormatters: IFormatter
    {
        public string Format(string key, string value)
        {
            return string.IsNullOrEmpty(value) ? key : $@"{key}\{value}";
        }
    }
}