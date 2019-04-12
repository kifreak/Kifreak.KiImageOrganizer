using System;
using System.Globalization;
using Kifreak.KiImageOrganizer.Console.Services;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class ByDate: SubFolderDecorator
    {
        private readonly string _format;
        
        public ByDate(SubFolders subFolders, MetadataService metadata, string format) : base(subFolders, metadata)
        {
            _format = format;
        }
        public override string GetSubFolder(IFormatter formatter)
        {
            return formatter.Format(_subFolders.GetSubFolder(formatter), GetSubFolderFromAction());
        }

        private string GetSubFolderFromAction()
        {
            string createdTime = _metadata.GetKey("Date/Time Original");
            bool isParsed = DateTime.TryParseExact(createdTime, "yyyy:MM:dd HH:mm:ss",CultureInfo.CurrentCulture,DateTimeStyles.None,out DateTime dateTime);
            return dateTime.ToString(_format);
        }
    }
}