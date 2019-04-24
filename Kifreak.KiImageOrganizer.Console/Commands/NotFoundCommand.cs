using Kifreak.KiImageOrganizer.Console.CommandFactory;
using Kifreak.KiImageOrganizer.Console.Helpers;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.Console.Commands
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