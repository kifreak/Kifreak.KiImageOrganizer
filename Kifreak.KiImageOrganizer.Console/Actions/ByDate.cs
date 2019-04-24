using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;
using System;
using System.Globalization;
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
            if (createdTime == null)
            {
                return await ActionHelpers.ExecuteWithAlternative(_model.Alternative, "NoDate");
            }
            DateTime.TryParseExact(createdTime, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dateTime);
            return dateTime.ToString(_model.Type.Type);
        }
    }
}