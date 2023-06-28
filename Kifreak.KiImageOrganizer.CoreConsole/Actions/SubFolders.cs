using Kifreak.KiImageOrganizer.CoreConsole.Formatters;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.CoreConsole.Actions
{
    public abstract class SubFolders
    {
        public abstract Task<string> GetSubFolder(IFormatter formatter);
    }
}