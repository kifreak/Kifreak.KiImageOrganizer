using System;
using System.Collections.Generic;
using System.Linq;
using MetaDataFileInfo.Classes;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class ByDate: SubFolderDecorator
    {
        private readonly string _format;
        
        public ByDate(SubFolders subFolders, List<KeyValuePair<string, Property>> metadata, string format) : base(subFolders, metadata)
        {
            _format = format;
        }
        public override string GetSubFolder()
        {
            return $@"{_subFolders.GetSubFolder()}\{GetSubFolderFromAction()}";
        }

        private string GetSubFolderFromAction()
        {
            KeyValuePair<string, Property> createdTime = _metadata.FirstOrDefault(t => t.Key.Equals("Created Content", StringComparison.OrdinalIgnoreCase));
            if (createdTime.Value == null)
            {
                return "NO_DATE";
            }

            return ((DateTime) createdTime.Value.Value).ToString(_format);
        }

    }
}