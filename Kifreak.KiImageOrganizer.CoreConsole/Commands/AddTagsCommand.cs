using System.Collections.Generic;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.CoreConsole.Actions;
using Kifreak.KiImageOrganizer.CoreConsole.CommandFactory;
using Kifreak.KiImageOrganizer.CoreConsole.Configuration;
using Kifreak.KiImageOrganizer.CoreConsole.Formatters;
using Kifreak.KiImageOrganizer.CoreConsole.Helpers;
using Kifreak.KiImageOrganizer.CoreConsole.Services;

namespace Kifreak.KiImageOrganizer.CoreConsole.Commands
{
    public class AddTagsCommand : ICommand, ICommandFactory
    {
        private IActionService _actionService;
        private readonly ParameterParser _parameterParser;
        private readonly IMetadataService _metadataService;

        public string Directory { get; set; }
        public string[] ByLabels { get; set; }

        public Dictionary<string, string> OptionsDescription => CommandsHelper.GetBasicTags();

        public AddTagsCommand(IActionService actionService, ParameterParser parameterParser, IMetadataService metadataService)
        {
            _actionService = actionService;
            _parameterParser = parameterParser;
            _metadataService = metadataService;
        }

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

        public string CommandName => "AddTags";
        public string Description => "Add tags to metadata for future proposes";

        public ICommand MakeCommand(string[] arguments)
        {
            string[] parameters = _parameterParser.GetParameters(arguments, 2, 2);
            var renameFilesCommand = Config.Get<AddTagsCommand>();
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
            CommandsHelper.ForeachFiles(Directory, async file => { AddTags(file, await GetNewFileName(file)); });
        }

        private void AddTags(string file, string tagList)
        {
            _metadataService.WriteInformation(file, tagList);
        }

        private async Task<string> GetNewFileName(string file)
        {
            string newFile = await _actionService.GetSubFolder(file, ByLabels,
                new MainFolder(string.Empty),
                new SeparatedByCommasFormatters());
            return newFile;
        }

        #endregion Private Methods
    }
}