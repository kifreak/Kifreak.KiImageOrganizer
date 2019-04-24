using System;
using System.Globalization;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class ByDate: SubFolderDecorator
    {
        private readonly ActionModel _model;


        public ByDate(ActionModel model) : base(model.Folders)
        {
            _model = model;
        }

      public override async Task<string> GetSubFolder(IFormatter formatter)
        {
            return formatter.Format(await _subFolders.GetSubFolder(formatter),await GetSubFolderFromAction(formatter));
        }

        private async Task<string> GetSubFolderFromAction(IFormatter formatter)
        {
            string createdTime = _model.MetadataService.GetKey("Date/Time Original");
            if (createdTime == null && _model.Alternative != null) return await _model.Alternative.GetSubFolder(formatter);
            bool isParsed = DateTime.TryParseExact(createdTime, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dateTime);
            return dateTime.ToString(_model.Type.Type);

        }
    }
}