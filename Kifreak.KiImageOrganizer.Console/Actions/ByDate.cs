using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Helpers;
using Kifreak.KiImageOrganizer.Console.Services;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class ByDate : SubFolderDecorator
    {
        private readonly ActionModel _model;
        private readonly IRegexService _regexService;

        public ByDate(ActionModel model, IRegexService regexService) : base(model.Folders)
        {
            _model = model;
            _regexService = Config.Get<IRegexService>();
        }

        public override async Task<string> GetSubFolder(IFormatter formatter)
        {
            return formatter.Format(await _subFolders.GetSubFolder(formatter), await GetSubFolderFromAction());
        }

        private async Task<string> GetSubFolderFromAction()
        {
            string createdTime = _model.MetadataService.GetKey(Config.UserConfig.MetadataDateTimeOriginal);
            if (string.IsNullOrEmpty(createdTime))
            {
                createdTime = _regexService.GetDateFromFileName(_model.File.Name);
                if (string.IsNullOrEmpty(createdTime))
                {
                    return await ActionHelpers.ExecuteWithAlternative(_model.Alternative, Config.UserConfig.ByDateDefaultText);
                }
            }
            DateTime.TryParseExact(createdTime, Config.UserConfig.DateTimeToOrganizeImagesFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dateTime);
            return dateTime.ToString(_model.Type.Type);
        }
    }
}