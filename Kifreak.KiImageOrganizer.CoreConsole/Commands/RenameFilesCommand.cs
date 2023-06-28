using System.Collections.Generic;
using Kifreak.KiImageOrganizer.CoreConsole.Actions;
using Kifreak.KiImageOrganizer.CoreConsole.CommandFactory;
using Kifreak.KiImageOrganizer.CoreConsole.Configuration;
using Kifreak.KiImageOrganizer.CoreConsole.Formatters;
using Kifreak.KiImageOrganizer.CoreConsole.Helpers;
using Kifreak.KiImageOrganizer.CoreConsole.Services;
using System.IO;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.CoreConsole.Commands
{
    public class RenameFilesCommand : ICommand, ICommandFactory
    {
        public RenameFilesCommand(IActionService actionService, ParameterParser parameterParser)
        {
            _actionService = actionService;
            _parameterParser = parameterParser;
        }

        public string Directory { get; set; }
        public string[] ByLabels { get; set; }

        public Dictionary<string, string> OptionsDescription => CommandsHelper.GetBasicTags();

        private readonly IActionService _actionService;
        private readonly ParameterParser _parameterParser;

        #region ICommand

        public Task Execute()
        {
            Run();
            return Task.CompletedTask;
        }

        public bool Validate()
        {
            if (ByLabels == null || ByLabels.Length == 0 || !System.IO.Directory.Exists(Directory))
            {
                ConsoleHelper.Error("Need parameter to execute this action and have a valid Directory.");
                System.Console.WriteLine("Rewrite Images Info: ");
                ConsoleHelper.Info(Description);
                return false;
            }

            if (!_actionService.HasNoExistAction(ByLabels)) return true;
            ConsoleHelper.Error("There are labels that are not accepted");
            ConsoleHelper.Info(Description);
            return false;
        }

        #endregion ICommand

        #region ICommandFactory

        public string CommandName => "RenameFiles";

        public string Description =>
            $@"Rename files. Params Actions:  [{_actionService.ActionsToString()}]. Can make alternative in case your parameter doesn't exist (i.e. City|Village).  Not working in different types of parameters (like City|Hour). Example RenameFiles C:\MyFolder DateTime Restaurant City";

        public ICommand MakeCommand(string[] arguments)
        {
            string[] parameters = _parameterParser.GetParameters(arguments, 2, 2);
            var renameFilesCommand = Config.Get<RenameFilesCommand>();
            if (parameters == null)
            {
                return renameFilesCommand;
            }
            renameFilesCommand.Directory = arguments[1];
            renameFilesCommand.ByLabels = parameters;
            return renameFilesCommand;
        }

        #endregion ICommandFactory

        #region Private Methods

        private void Run()
        {
            CommandsHelper.ForeachFiles(Directory, async file => { RenameFile(file, await GetNewFileName(file)); });
        }

        private void RenameFile(string file, string newFile)
        {
            var fileInfo = new FileInfo(file);
            var newFileCompletePath = $@"{fileInfo.DirectoryName}{Config.getDefaultPathSlash()}{newFile}{fileInfo.Extension}";
            if (File.Exists(newFileCompletePath))
            {
                ConsoleHelper.Info($"{newFile} already exist.");
                return;
            }
            ConsoleHelper.Info($"{file} named as {newFile}");
            File.Move(file, newFileCompletePath);
        }

        private async Task<string> GetNewFileName(string file)
        {
            string newFile = await _actionService.GetSubFolder(file, ByLabels,
                new MainFolder(string.Empty),
                new FileFormatters());
            return newFile;
        }

        #endregion Private Methods
    }
}