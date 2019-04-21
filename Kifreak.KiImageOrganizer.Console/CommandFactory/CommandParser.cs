using System;
using System.Collections.Generic;
using System.Linq;
using Kifreak.KiImageOrganizer.Console.Commands;

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
                return new NotFoundCommand { Name = args[0] };
            }

            return command.MakeCommand(args);
        }

        private ICommandFactory FindRequestCommand(string commandName)
        {
            return _availableCommands.FirstOrDefault(t => string.Equals(t.CommandName,commandName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}