using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.CommandFactory;
using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Helpers;
using Kifreak.KiImageOrganizer.Console.Services;

namespace Kifreak.KiImageOrganizer.Console.Commands
{
    public class AddTagsCommand : ICommand, ICommandFactory
    {
        private ActionService _actionService;
        private readonly ParameterParser _parameterParser;
        private readonly IMetadataService _metadataService;

        public string Directory { get; set; }
        public string[] ByLabels { get; set; }

        public AddTagsCommand(ActionService actionService, ParameterParser parameterParser, IMetadataService metadataService)
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