using System.IO;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.CommandFactory;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Helpers;
using Kifreak.KiImageOrganizer.Console.Services;

namespace Kifreak.KiImageOrganizer.Console.Commands
{
    public class OrganizerImagesCommand : ICommand, ICommandFactory
    {
        public OrganizerImagesCommand()
        {
            _actionService = new ActionService();
        }
        public string Directory { get; set; }
        public string[] ByLabels { get; set; }

        private readonly ActionService _actionService;

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

            if (_actionService.HasAllAction(ByLabels))
            {
                ConsoleHelper.Error("There are labels that are not accepted");
                ConsoleHelper.Info(Description);
                return false;
            }

            return true;

        }
        #endregion

        #region CommandFactory
        public string CommandName => "OrganizerImages";
        public string Description => $@"Organize Images. Params Actions: [{_actionService.ActionsToString()}]. Can make alternative in case your parameter doesn't exist (i.e. City|Village). Not working in different types of parameters (like City|Hour). Example: c:\MyFolder City Date";

        public ICommand MakeCommand(string[] arguments)
        {
            ParameterParser parser = new ParameterParser();
            string[] parameters = parser.GetParameters(arguments, 2, 2);
            if (parameters == null)
            {
                return new OrganizerImagesCommand();
            }

            return new OrganizerImagesCommand
            {
                Directory = arguments[1],
                ByLabels = parameters
            };
        }

        #endregion

            #region Private Methods
            private void Run()
            {
                CommandsHelper.ForeachFiles(Directory, (file) =>
                {
                    string newFolder = GetFileFolder(file);
                    CreateFolderIfIsNecessary(newFolder);
                    MoveFileToNewPath(file, newFolder);
                });
            }
            private string GetFileFolder(string file)
            {
                string newFolder =  _actionService.GetSubFolder(file,ByLabels, 
                    new MainFolder($@"{Directory}\Organized"),
                    new FolderFormatters());
                ConsoleHelper.Info($"{file} copy to {newFolder}");
                return newFolder;
            }
        
            private void CreateFolderIfIsNecessary(string newFolder)
            {
                System.IO.Directory.CreateDirectory(newFolder);
            }

            private void MoveFileToNewPath(string fileName,string newFolder)
            {
                string newFile = $@"{newFolder}\{Path.GetFileName(fileName)}";
                if (!File.Exists(newFile))
                {
                    File.Copy(fileName,newFile);
                } else 
                {
                    ConsoleHelper.Error($"{newFolder} already exist");
                }

            }
            #endregion
    }
   
}