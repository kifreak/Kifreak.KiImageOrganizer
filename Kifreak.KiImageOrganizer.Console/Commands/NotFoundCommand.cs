using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.CommandFactory;

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