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
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-UK");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-UK");
            var availableCommands = GetAvalaibleCommands();
            if (args.Length == 0)
            {
                PrintUsage(availableCommands);
                return;
            }
            var parser = new CommandParser(availableCommands);
            var command = parser.ParseCommand(args);

            if (command.Validate())
            {
                await command.Execute();
            }

            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
            System.Console.WriteLine("Program ended");
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.ReadKey();
            

        }
        static IEnumerable<ICommandFactory> GetAvalaibleCommands()
        {
            return new ICommandFactory[] { new OrganizerImagesCommand() };
            //TODO Add command for rename the files with Date and places.
        }
        private static void PrintUsage(IEnumerable<ICommandFactory> availableCommands)
        {
            System.Console.WriteLine("Usage: Commands available");
            System.Console.WriteLine("Commands:");
            foreach (var command in availableCommands)
                ConsoleHelper.Description($"{command.CommandName}: {command.Description}");
            System.Console.ReadKey();
        }
    }
}
