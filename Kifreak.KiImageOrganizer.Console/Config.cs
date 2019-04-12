using System;
using System.Collections.Generic;
using Kifreak.KiImageOrganizer.Console.CommandFactory;
using Kifreak.KiImageOrganizer.Console.Commands;

namespace Kifreak.KiImageOrganizer.Console
{
    public static class Config
    {
        public static DateTime LastCallToOSM { get; set; }

        public static IEnumerable<ICommandFactory> GetAvailableCommands()
        {
            return new ICommandFactory[] { new OrganizerImagesCommand(), new RenameFilesCommands() };
        }
    }
}