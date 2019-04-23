using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class Noop:SubFolderDecorator
    {
        public Noop(ActionModel model):base(model.Folders) { }

        public override async Task<string> GetSubFolder(IFormatter formatter)
        {
            return string.Empty;

        }
    }
}