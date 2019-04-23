using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Services;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public abstract class SubFolders
    {
        public abstract Task<string> GetSubFolder(IFormatter formatter);
    }

}