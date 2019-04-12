using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Services;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public abstract class SubFolders
    {
        public abstract string GetSubFolder(IFormatter formatter);
        protected MetadataService Metadata;
        protected string Alternative;

        public void SetMetaData(MetadataService metadataService)
        {
            Metadata = metadataService;
        }

        public void SetAlternative(string alternative)
        {
            Alternative = alternative;
        }
    }
}