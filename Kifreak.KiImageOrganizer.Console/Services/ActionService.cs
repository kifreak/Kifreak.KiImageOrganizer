﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;

namespace Kifreak.KiImageOrganizer.Console.Services
{
    public class ActionService
    {
        private SubFolders _subFolders;
        public Dictionary<string, Func<SubFolders, SubFolders>> ActionList => new Dictionary<string, Func<SubFolders, SubFolders>>
        {
            
            {"City" ,(folders) => new City("city",folders)},
            {"Road" ,(folders) => new City("road",folders)},
            {"Restaurant" ,(folders) => new City("restaurant",folders)},
            {"Village" ,(folders) => new City("village",folders)},
            {"Country" ,(folders) => new City("country",folders)},
            {"County" ,(folders) => new City("county",folders)},
            {"AmenityType", (folders) => new City("AmenityType",folders) },
            {"AmenityName", (folders) => new City("AmenityName",folders) },
            {"Date", (folders)=> new ByDate(folders,"yyyy-MM-dd")},
            {"DateTime", (folders)=> new ByDate(folders,"yyyy-MM-dd HH_mm_ss")},
            {"Time", (folders)=> new ByDate(folders,"HH_mm_ss")},
            {"YearMonth", (folders) => new ByDate(folders,"yyyy-MM") }
        };
        //City|DateTime
        public MetadataService MetadataInfo { get; private set; }

        public bool HasNoExistAction(string[] actions)
        {
            return actions.Any(t => !ActionList.ContainsKey(t.Split('|')[0]));
        }

        public string ActionsToString()
        {
            return string.Join(",", ActionList.Select(t => t.Key).ToList());
        }

        public async Task<string> GetSubFolder(string file, string[] labels, SubFolders subFolder,IFormatter formatter)
        {
            GetMetadataInfo(file);
            _subFolders = subFolder;
            foreach (string label in labels)
            {
                var keyAlternative = GetKeysFromLabel(label);
                _subFolders = ActionList[keyAlternative.Key].Invoke(_subFolders);
               _subFolders.SetAlternative(keyAlternative.Alternative);
               _subFolders.SetMetaData(MetadataInfo);
            }

            return await _subFolders.GetSubFolder(formatter);

        }

        private KeysAlternatives GetKeysFromLabel(string label)
        {
            string[] keys = label.Split('|');
            return new KeysAlternatives
            {
                Key = keys.First(),
                Alternative = keys.Skip(1).LastOrDefault()
            };
        }

        private void GetMetadataInfo(string file)
        {
            MetadataInfo = new MetadataService(file);
        }
}
}