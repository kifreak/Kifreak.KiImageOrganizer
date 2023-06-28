
using Kifreak.KiImageOrganizer.CoreConsole.CommandFactory;
using Kifreak.KiImageOrganizer.CoreConsole.Commands;
using Kifreak.KiImageOrganizer.CoreConsole.Configuration;
using Kifreak.KiImageOrganizer.CoreConsole.Helpers;

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