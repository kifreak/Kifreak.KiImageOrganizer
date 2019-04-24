using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Formatters;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public abstract class SubFolders
    {
        public abstract Task<string> GetSubFolder(IFormatter formatter);
    }

}