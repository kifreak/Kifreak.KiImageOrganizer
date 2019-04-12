using System.ComponentModel;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Services;


namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class SubFolderDecorator : SubFolders
    {

        protected SubFolders _subFolders;
        public SubFolderDecorator(SubFolders subFolders)
        {
            _subFolders = subFolders;
        }

        public override string GetSubFolder(IFormatter formatter)
        {
            return _subFolders.GetSubFolder(formatter);
        }
    }
}