using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Kifreak.KiImageOrganizer.Console.Services;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class ActionService
    {
        private SubFolders _subFolders;
        public Dictionary<string, Func<SubFolders, MetadataService, SubFolders>> ActionList => new Dictionary<string, Func<SubFolders, MetadataService, SubFolders>>
        {
            {"City" ,(folders, metadata) => new City("city",folders, metadata)},
            {"Road" ,(folders, metadata) => new City("road",folders, metadata)},
            {"Restaurant" ,(folders, metadata) => new City("restaurant",folders, metadata)},
            {"Village" ,(folders, metadata) => new City("village",folders, metadata)},
            {"Country" ,(folders, metadata) => new City("country",folders, metadata)},
            {"County" ,(folders, metadata) => new City("county",folders, metadata)},
            {"DateTime", (folders,metadata)=> new ByDate(folders, metadata,"yyyy-MM-dd")},
            {"YearMonth", (folders,metadata) => new ByDate(folders,metadata,"yyyy-MM") }
        };
        public MetadataService MetadataInfo { get; private set; }

        public bool HasAllAction(string[] actions)
        {
            return actions.Any(t => !ActionList.ContainsKey(t));
        }

        public string ActionsToString()
        {
            return string.Join(",", ActionList.Select(t => t.Key).ToList());
        }

        public string GetSubFolder(string file, string[] labels, SubFolders subFolder,IFormatter formatter)
        {
            GetMetadataInfo(file);
            _subFolders = subFolder;
            foreach (string label in labels)
            {
                _subFolders = ActionList[label].Invoke(_subFolders, MetadataInfo);
            }

            return _subFolders.GetSubFolder(formatter);
        }

        private void GetMetadataInfo(string file)
        {
            MetadataInfo = new MetadataService(file);
        }
}
}