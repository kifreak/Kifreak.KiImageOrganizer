using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.Console.Actions
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