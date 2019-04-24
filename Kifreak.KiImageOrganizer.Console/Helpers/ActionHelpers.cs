using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Formatters;

namespace Kifreak.KiImageOrganizer.Console.Helpers
{
    public static class ActionHelpers
    {
        public static async Task<string> ExecuteWithAlternative(SubFolders alternative, string defaultString)
        {
            return alternative == null
                ? defaultString
                : await alternative.GetSubFolder(Config.Get<JustValueFormatters>());
        }
    }
}