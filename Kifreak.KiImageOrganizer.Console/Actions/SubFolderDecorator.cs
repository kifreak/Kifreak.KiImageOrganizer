﻿using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Services;


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
        public override string GetSubFolder(IFormatter formatter)
        {
            return _subFolders.GetSubFolder(formatter);
        }
    }
}