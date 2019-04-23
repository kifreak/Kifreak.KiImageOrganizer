using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;

namespace Kifreak.KiImageOrganizer.Console.Services
{
    public class ActionService
    {
        private readonly IActionParser _parser;
        
        private SubFolders _subFolders;
      
        public ActionService(IActionParser parser)
        {
            _parser = parser;
        }
        

        public string ActionsToString()
        {
            return _parser.ToString();
        }

        public bool HasNoExistAction(string[] actions)
        {
            return _parser.HasNoExistAction(actions);
        }
        public async Task<string> GetSubFolder(string file, string[] labels, SubFolders subFolder,IFormatter formatter)
        {
            _subFolders = subFolder;
            foreach (string label in labels)
            {
                KeysAlternatives keyAlternative = GetKeysFromLabel(label);
               _subFolders = _parser.InvokeWithAlternative(keyAlternative, _subFolders,file);
            }

            return await _subFolders.GetSubFolder(formatter);

        }

        private KeysAlternatives GetKeysFromLabel(string label)
        {
            string[] keys = label.Split(Config.AlternativeCharacter);
            return new KeysAlternatives
            {
                Key = keys.First(),
                Alternative = keys.Skip(1).LastOrDefault()
            };
        }
    }
}