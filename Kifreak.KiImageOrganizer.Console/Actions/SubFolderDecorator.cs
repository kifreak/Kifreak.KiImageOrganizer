using Kifreak.KiImageOrganizer.Console.Formatters;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class SubFolderDecorator : SubFolders
    {
        protected SubFolders _subFolders;

        public SubFolderDecorator(SubFolders subFolders)
        {
            _subFolders = subFolders;
        }

        public override Task<string> GetSubFolder(IFormatter formatter)
        {
            return _subFolders.GetSubFolder(formatter);
        }
    }
}