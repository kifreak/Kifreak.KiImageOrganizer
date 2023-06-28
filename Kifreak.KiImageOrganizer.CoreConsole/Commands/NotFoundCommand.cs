using Kifreak.KiImageOrganizer.CoreConsole.CommandFactory;
using Kifreak.KiImageOrganizer.CoreConsole.Helpers;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.CoreConsole.Commands
{
    public class NotFoundCommand : ICommand
    {
        public string Name { get; set; }

        public Task Execute()
        {
            ConsoleHelper.Error($"Couldn't find command: {Name}");
            return Task.CompletedTask;
        }

        public bool Validate()
        {
            return true;
        }
    }
}