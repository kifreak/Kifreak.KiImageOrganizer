using System;
using System.Globalization;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Services;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class ByDate: SubFolderDecorator
    {
        private readonly string _format;
        
        public ByDate(SubFolders subFolders, string format) : base(subFolders)
        {
            _format = format;
        }
        public override string GetSubFolder(IFormatter formatter)
        {
            return formatter.Format(_subFolders.GetSubFolder(formatter), GetSubFolderFromAction());
        }

        private string GetSubFolderFromAction()
        {
            string createdTime = Metadata.GetKey("Date/Time Original");
            bool isParsed = DateTime.TryParseExact(createdTime, "yyyy:MM:dd HH:mm:ss",CultureInfo.CurrentCulture,DateTimeStyles.None,out DateTime dateTime);
            return dateTime.ToString(_format);
        }
    }
}