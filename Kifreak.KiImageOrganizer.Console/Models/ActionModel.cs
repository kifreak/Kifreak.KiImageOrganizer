using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.Services;

namespace Kifreak.KiImageOrganizer.Console.Models
{
    public class ActionModel
    {
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