using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.CoreConsole.Actions;
using Kifreak.KiImageOrganizer.CoreConsole.Configuration;
using Kifreak.KiImageOrganizer.CoreConsole.Formatters;

namespace Kifreak.KiImageOrganizer.CoreConsole.Helpers
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