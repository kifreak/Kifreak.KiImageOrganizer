using Kifreak.KiImageOrganizer.Console.CommandFactory;
using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Commands;

namespace Kifreak.KiImageOrganizer.Console
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Config.Startup();
            var availableCommands = Config.GetAvailableCommands();
            if (args.Length == 0)
            {
                var helpCommand = Config.Get<HelpCommand>();
                await helpCommand.Execute();
                return;
            }
            var parser = Config.Get<CommandParser>("availableCommands", availableCommands);
            ICommand command = parser.ParseCommand(args);
            if (command.Validate())
            {
                await command.Execute();
            }
            ConsoleHelper.EndProgram();
        }
    }
}