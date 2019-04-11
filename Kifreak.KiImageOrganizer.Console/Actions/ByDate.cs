using System;
using System.Collections.Generic;
using System.Linq;
using MetaDataFileInfo.Classes;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class ByDate: SubFolderDecorator
    {
        public ByDate(SubFolders subFolders, List<KeyValuePair<string, Property>> metadata) : base(subFolders, metadata)
        {
        }
        public override string GetSubFolder()
        {
            return $@"{_subFolders.GetSubFolder()}\{GetSubFolderFromAction()}";
        }

        private string GetSubFolderFromAction()
        {
            KeyValuePair<string, Property> createdTime = _metadata.FirstOrDefault(t => t.Key.Equals("Contenido creado", StringComparison.OrdinalIgnoreCase));
            if (createdTime.Value == null)
            {
                return "NO_DATE";
            }

            return ((DateTime) createdTime.Value.Value).ToString("yyyy-MM-dd");
        }

    }
}