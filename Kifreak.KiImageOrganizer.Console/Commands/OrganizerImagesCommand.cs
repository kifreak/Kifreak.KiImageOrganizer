using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.CommandFactory;
using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Helpers;
using Kifreak.KiImageOrganizer.Console.Services;
using System.IO;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.Console.Commands
{
    public class OrganizerImagesCommand : ICommand, ICommandFactory
    {
        public OrganizerImagesCommand(ActionService actionService, ParameterParser parameterParser)
        {
            _actionService = actionService;
            _parameterParser = parameterParser;
        }

        public string Directory { get; set; }
        public string[] ByLabels { get; set; }

        private readonly ActionService _actionService;
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
                System.Console.WriteLine("Organizer Images Info: ");
                ConsoleHelper.Info(Description);
                return false;
            }

            if (_actionService.HasNoExistAction(ByLabels))
            {
                ConsoleHelper.Error("There are labels that are not accepted");
                ConsoleHelper.Info(Description);
                return false;
            }

            return true;
        }

        #endregion ICommand

        #region CommandFactory

        public string CommandName => "OrganizerImages";
        public string Description => $@"Organize Images. Params Actions: [{_actionService.ActionsToString()}]. Can make alternative in case your parameter doesn't exist (i.e. City|Village). Not working in different types of parameters (like City|Hour). Example: c:\MyFolder City Date";

        public ICommand MakeCommand(string[] arguments)
        {
            string[] parameters = _parameterParser.GetParameters(arguments, 2, 2);
            var organizerImageCommand = Config.Get<OrganizerImagesCommand>();
            if (parameters == null)
            {
                return organizerImageCommand;
            }

            organizerImageCommand.Directory = arguments[1];
            organizerImageCommand.ByLabels = parameters;
            return organizerImageCommand;
        }

        #endregion CommandFactory

        #region Private Methods

        private void Run()
        {
            CommandsHelper.ForeachFiles(Directory, async (file) =>
            {
                string newFolder = await GetFileFolder(file);
                CreateFolderIfIsNecessary(newFolder);
                MoveFileToNewPath(file, newFolder);
            });
        }

        private async Task<string> GetFileFolder(string file)
        {
            string newFolder = await _actionService.GetSubFolder(file, ByLabels,
                Config.Get<MainFolder>("path", $@"{Directory}\Organized"),
                Config.Get<FolderFormatters>());
            ConsoleHelper.Info($"{file} copy to {newFolder}");
            return newFolder;
        }

        private void CreateFolderIfIsNecessary(string newFolder)
        {
            System.IO.Directory.CreateDirectory(newFolder);
        }

        private void MoveFileToNewPath(string fileName, string newFolder)
        {
            string newFile = $@"{newFolder}\{Path.GetFileName(fileName)}";
            if (!File.Exists(newFile))
            {
                File.Copy(fileName, newFile);
            }
            else
            {
                ConsoleHelper.Error($"{newFolder} already exist");
            }
        }

        #endregion Private Methods
    }
}