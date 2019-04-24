using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kifreak.KiImageOrganizer.Console.Services
{
    public class ActionParser : IActionParser
    {
        private readonly Dictionary<string, ActionExecutionModel> _actionType = new Dictionary<string, ActionExecutionModel>
        {
            { "City",new ActionExecutionModel { Class = "City", Type ="City"}},
            { "Road", new ActionExecutionModel { Class = "City", Type ="Road"}},
            { "Village",new ActionExecutionModel { Class = "City", Type ="Village"}},
            { "Country",new ActionExecutionModel { Class = "City", Type ="Country"}},
            { "County",new ActionExecutionModel { Class = "City", Type ="County"}},
            { "AmenityType",new ActionExecutionModel { Class = "City", Type ="AmenityType"}},
            { "AmenityName",new ActionExecutionModel { Class = "City", Type ="AmenityName"}},
            { "Date",new ActionExecutionModel { Class = "ByDate", Type ="yyyy-MM-dd"}},
            { "DateTime",new ActionExecutionModel { Class = "ByDate", Type ="yyyy-MM-dd HH_mm_ss"}},
            { "Time",new ActionExecutionModel { Class = "ByDate", Type ="HH_mm_ss"}},
            { "YearMonth",new ActionExecutionModel { Class = "ByDate", Type ="yyyy-MM"}},
            { "Noop",new ActionExecutionModel { Class = "Noop", Type =string.Empty}},
        };

        public bool HasNoExistAction(string[] actions)
        {
            return actions.Any(t => !_actionType.ContainsKey(t.Split(Config.AlternativeCharacter)[0]));
        }

        public SubFolders InvokeWithAlternative(KeysAlternatives keysAlternatives, SubFolders subFolders, string filePath)
        {
            var metadataService = GetMetadataService(filePath);
            ActionModel model = new ActionModel
            {
                MetadataService = metadataService,
                Folders = subFolders,
                Type = _actionType[keysAlternatives.Key],
                Alternative = Instantiate(
                    new ActionModel
                    {
                        MetadataService = metadataService,
                        Folders = subFolders,
                        Type = keysAlternatives.Alternative == null ? null : _actionType[keysAlternatives.Alternative]
                    })
            };
            return Instantiate(model);
        }

        public override string ToString()
        {
            return string.Join(",", _actionType.Select(t => t.Key));
        }

        private SubFolders Instantiate(ActionModel model)
        {
            if (model.Type == null)
            {
                return null;
            }
            Type type = Type.GetType($"Kifreak.KiImageOrganizer.Console.Actions.{model.Type.Class}");

            return (SubFolders)Activator.CreateInstance(type ?? throw new InvalidOperationException(), model);
        }

        private IMetadataService GetMetadataService(string filePath)
        {
            var metadataService = Config.Get<IMetadataService>();
            metadataService.SetFileInformation(filePath);
            return metadataService;
        }
    }
}