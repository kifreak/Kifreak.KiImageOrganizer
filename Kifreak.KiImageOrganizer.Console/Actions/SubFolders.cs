using System.Runtime.Serialization;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public abstract class SubFolders
    {
        public abstract string GetSubFolder(IFormatter formatter);
    }
}