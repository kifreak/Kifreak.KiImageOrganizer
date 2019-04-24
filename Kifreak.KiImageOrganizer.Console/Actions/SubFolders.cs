using Kifreak.KiImageOrganizer.Console.Formatters;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public abstract class SubFolders
    {
        public abstract Task<string> GetSubFolder(IFormatter formatter);
    }
}