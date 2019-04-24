using Kifreak.KiImageOrganizer.Console.Commands;
using Kifreak.KiImageOrganizer.Console.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kifreak.KiImageOrganizer.Console.CommandFactory
{
    public class CommandParser
    {
        private readonly IEnumerable<ICommandFactory> _availableCommands;

        public CommandParser(IEnumerable<ICommandFactory> availableCommands)
        {
            _availableCommands = availableCommands;
        }

        internal ICommand ParseCommand(string[] args)
        {
            var command = FindRequestCommand(args[0]);
            if (command == null)
            {
                var notFoundCommand = Config.Get<NotFoundCommand>();
                notFoundCommand.Name = args[0];
                return notFoundCommand;
            }

            return command.MakeCommand(args);
        }

        private ICommandFactory FindRequestCommand(string commandName)
        {
            return _availableCommands.FirstOrDefault(t => string.Equals(t.CommandName, commandName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}