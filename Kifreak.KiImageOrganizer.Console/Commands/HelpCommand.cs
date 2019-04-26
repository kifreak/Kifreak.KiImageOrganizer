using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.CommandFactory;
using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Helpers;

namespace Kifreak.KiImageOrganizer.Console.Commands
{
    public class HelpCommand : ICommand, ICommandFactory
    {
        #region ICommand

        public string Topic { get; set; }

        public Task Execute()
        {
            if (string.IsNullOrEmpty(Topic))
            {
                ShowBasicHelp();
            }
            else
            {
                ShowSpecificHelp();
            }

            return Task.CompletedTask;
        }

        public void ShowBasicHelp()
        {
            ConsoleHelper.JumpLine(1);
            System.Console.WriteLine("KiOrganizer is a program to organize your images based in its metadata information.");
            System.Console.WriteLine("If you need more information about an action. Write:");
            ConsoleHelper.WriteLineDarkGreen("help actionName");
            ConsoleHelper.JumpLine(1);
            IEnumerable<ICommandFactory> availableCommands = Config.GetAvailableCommands();
            foreach (var availableCommand in availableCommands)
            {
                ConsoleHelper.WriteLineSeparator();
                ConsoleHelper.WriteDarkYellow($"{availableCommand.CommandName}: ");
                System.Console.WriteLine(availableCommand.Description);
            }
        }

        public void ShowSpecificHelp()
        {
            IEnumerable<ICommandFactory> availableCommands = Config.GetAvailableCommands();
            ICommandFactory command = availableCommands.FirstOrDefault(
                t => t.CommandName.Equals(Topic, StringComparison.CurrentCultureIgnoreCase));
            if (command == null)
            {
                ConsoleHelper.Error("The introduced command doesn't exist.");
                return;
            }
            ConsoleHelper.WriteLineSeparator();
            ConsoleHelper.WriteDarkYellow($"{command.CommandName}: ");
            System.Console.WriteLine(command.Description);
            if (command.OptionsDescription == null || command.OptionsDescription.Count == 0)
            {
                ConsoleHelper.WriteLineDarkBlue("This command doesn't have parameters");
            }
            else
            {
                foreach (var option in command.OptionsDescription)
                {
                    ConsoleHelper.WriteDarkYellow($"{option.Key}: ");
                    System.Console.WriteLine(option.Value);
                }
            }
        }

        public bool Validate()
        {
            return true;
        }

        #endregion ICommand

        #region ICommandFactory

        public string CommandName => "Help";
        public string Description => "Show this help page.";

        public Dictionary<string, string> OptionsDescription => new Dictionary<string, string>
            { {"Command", "Write the command you need help with."}};

        public ICommand MakeCommand(string[] arguments)
        {
            var topic = string.Empty;
            if (arguments != null && arguments.Length > 1)
            {
                topic = arguments[1];
            }

            var helpCommand = Config.Get<HelpCommand>();
            helpCommand.Topic = topic;
            return helpCommand;
        }

        #endregion ICommandFactory
    }
}