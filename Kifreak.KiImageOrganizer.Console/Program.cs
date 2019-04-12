using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.CommandFactory;
using Kifreak.KiImageOrganizer.Console.Commands;

namespace Kifreak.KiImageOrganizer.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var availableCommands = Config.GetAvailableCommands();
            if (args.Length == 0)
            {
                PrintUsage(availableCommands);
                return;
            }
            var parser = new CommandParser(availableCommands);
            ICommand command = parser.ParseCommand(args);

            if (command.Validate())
            {
                await command.Execute();
            }
            ConsoleHelper.EndProgram();
        }
      
        private static void PrintUsage(IEnumerable<ICommandFactory> availableCommands)
        {
            System.Console.WriteLine("Usage: Commands available");
            System.Console.WriteLine("Commands:");
            foreach (var command in availableCommands)
                ConsoleHelper.Info($"{command.CommandName}: {command.Description}");
            System.Console.ReadKey();
        }
    }
}
