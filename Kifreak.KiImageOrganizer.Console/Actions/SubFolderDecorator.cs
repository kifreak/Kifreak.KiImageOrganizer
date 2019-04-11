using System.Collections.Generic;
using MetaDataFileInfo.Classes;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class SubFolderDecorator : SubFolders
    {
        protected SubFolders _subFolders;
        protected readonly List<KeyValuePair<string, Property>> _metadata;

        public SubFolderDecorator(SubFolders subFolders, List<KeyValuePair<string, Property>> metadata)
        {
            _subFolders = subFolders;
            _metadata = metadata;
        }
        public override string GetSubFolder()
        {
            return _subFolders.GetSubFolder();
        }
    }
}