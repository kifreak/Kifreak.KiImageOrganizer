using System.IO;
using System.Net;
using Kifreak.KiImageOrganizer.CoreConsole.Actions;
using Kifreak.KiImageOrganizer.CoreConsole.Services;

namespace Kifreak.KiImageOrganizer.CoreConsole.Models
{
    public class ActionModel
    {
        public FileInfo File { get; set; }
        public ActionExecutionModel Type { get; set; }
        public SubFolders Alternative { get; set; }
        public SubFolders Folders { get; set; }
        public IMetadataService MetadataService { get; set; }
    }

    public class ActionExecutionModel
    {
        public string Type { get; set; }
        public string Class { get; set; }
    }
}