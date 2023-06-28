using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Kifreak.KiImageOrganizer.CoreConsole.Configuration;

namespace Kifreak.KiImageOrganizer.CoreConsole.Services
{
    public class RegexService : IRegexService
    {
        public string GetDateFromFileName(string name)
        {
            var pattern = @"\d{8}";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(name);
            foreach (Match match in matches)
            {
                if (DateTime.TryParseExact(match.Value, Config.UserConfig.DateFromFileNameFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newCreatedTime))
                {
                    return newCreatedTime.ToString(Config.UserConfig.DateTimeToOrganizeImagesFormat);
                }
            }
            return null;
        }

        public string RemoveAllNotAllowedCharactersInAPath(string path)
        {
            var regex = new Regex("<|“|\"|:|>|\\||\\\\|/|\\?|\\*");
            return regex.Replace(path, "");
        }
    }
}