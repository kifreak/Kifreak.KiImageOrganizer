using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Helpers;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class ByDate : SubFolderDecorator
    {
        private readonly ActionModel _model;

        public ByDate(ActionModel model) : base(model.Folders)
        {
            _model = model;
        }

        public override async Task<string> GetSubFolder(IFormatter formatter)
        {
            return formatter.Format(await _subFolders.GetSubFolder(formatter), await GetSubFolderFromAction());
        }

        private async Task<string> GetSubFolderFromAction()
        {
            string createdTime = _model.MetadataService.GetKey("Date/Time Original");
            if (string.IsNullOrEmpty(createdTime))
            {


                createdTime = GetDateFromFileName(_model.File.Name);
                if (string.IsNullOrEmpty(createdTime))
                {
                    return await ActionHelpers.ExecuteWithAlternative(_model.Alternative, "NoDate");
                }
            }
            DateTime.TryParseExact(createdTime, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dateTime);
            return dateTime.ToString(_model.Type.Type);
        }

        private string GetDateFromFileName(string name)
        {
            var pattern = @"\d{8}";
            Regex regex = new Regex(pattern);
            var match = regex.Match(name);
            if (match != null && DateTime.TryParseExact(match.Value,"yyyymmdd",CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newCreatedTime))
            {
                return newCreatedTime.ToString("yyyy:MM:dd HH:mm:ss");
            }
            return null;
        }
    }
}