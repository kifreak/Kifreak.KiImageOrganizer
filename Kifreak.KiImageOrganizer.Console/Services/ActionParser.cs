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
        //protected Dictionary<string, Func<ActionModel>> ActionList => new Dictionary<string, Func<ActionModel>>
        //{
        //    {"City" ,(actionModel) => City.Init()
        //        City.Init("City",alternative, folders,metadataService)},
        //    {"Road" ,(folders,alternative,metadataService) => City.Init("Road",alternative,folders,metadataService)},
        //    {"Village" ,(folders,alternative,metadataService) => City.Init("Village",alternative,folders,metadataService)},
        //    {"Country" ,(folders,alternative,metadataService) => City.Init("Country",alternative,folders,metadataService)},
        //    {"County" ,(folders,alternative,metadataService) => City.Init("County",alternative,folders,metadataService)},
        //    {"AmenityType", (folders,alternative,metadataService) => City.Init("AmenityType",alternative,folders,metadataService) },
        //    {"AmenityName", (folders,alternative,metadataService) => City.Init("AmenityName",alternative,folders,metadataService) },
        //    {"Date", (folders,alternative,metadataService)=> ByDate.Init("yyyy-MM-dd",alternative,folders,metadataService)},
        //    {"DateTime", (folders,alternative,metadataService)=> ByDate.Init("yyyy-MM-dd HH_mm_ss",alternative,folders,metadataService)},
        //    {"Time", (folders,alternative,metadataService)=> ByDate.Init("HH_mm_ss",alternative,folders,metadataService)},
        //    {"YearMonth", (folders,alternative,metadataService) => ByDate.Init("yyyy-MM",alternative,folders,metadataService) },
        //    {"Noop", (folders,alternative,metadataServices) => new Noop(folders) }
        //};

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