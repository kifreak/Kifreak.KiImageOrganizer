﻿using System.Collections.Generic;

namespace Kifreak.KiImageOrganizer.Console.CommandFactory
{
    public interface ICommandFactory
    {
        string CommandName { get; }
        string Description { get; }

        Dictionary<string, string> OptionsDescription { get; }

        ICommand MakeCommand(string[] arguments);
    }
}