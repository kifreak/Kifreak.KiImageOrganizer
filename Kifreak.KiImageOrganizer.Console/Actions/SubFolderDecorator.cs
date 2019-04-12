using System.Collections.Generic;
using Kifreak.KiImageOrganizer.Console.Services;
using MetadataExtractor;


namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class SubFolderDecorator : SubFolders
    {
        protected SubFolders _subFolders;
        protected readonly MetadataService _metadata;

        public SubFolderDecorator(SubFolders subFolders, MetadataService metadata)
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