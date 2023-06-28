using Kifreak.KiImageOrganizer.CoreConsole.Formatters;
using Kifreak.KiImageOrganizer.CoreConsole.Models;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.CoreConsole.Actions
{
    public class Noop : SubFolderDecorator
    {
        public Noop(ActionModel model) : base(model.Folders)
        {
        }

        public override Task<string> GetSubFolder(IFormatter formatter)
        {
            return Task.FromResult(string.Empty);
        }
    }
}