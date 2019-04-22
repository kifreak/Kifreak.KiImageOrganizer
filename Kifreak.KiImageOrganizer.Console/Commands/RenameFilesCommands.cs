using System.IO;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.CommandFactory;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Helpers;
using Kifreak.KiImageOrganizer.Console.Services;

namespace Kifreak.KiImageOrganizer.Console.Commands
{
    public class RenameFilesCommands : ICommand, ICommandFactory
    {
        
        public RenameFilesCommands()
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
                System.Console.WriteLine("Rewrite Images Info: ");
                ConsoleHelper.Info(Description);
                return false;
            }

            if (!_actionService.HasAllAction(ByLabels)) return true;
            ConsoleHelper.Error("There are labels that are not accepted");
            ConsoleHelper.Info(Description);
            return false;

        }

        #endregion

        #region ICommandFactory

        public string CommandName => "RenameFiles";

        public string Description =>
            $@"Rename files. Params Actions:  [{_actionService.ActionsToString()}]. Can make alternative in case your parameter doesn't exist (i.e. City|Village).  Not working in different types of parameters (like City|Hour). Example RenameFiles C:\MyFolder DateTime Restaurant City";

        public CommandFactory.ICommand MakeCommand(string[] arguments)
        {
            ParameterParser parser = new ParameterParser();
            string[] parameters = parser.GetParameters(arguments, 2, 2);
            if (parameters == null)
            {
                return new RenameFilesCommands();
            }

            return new RenameFilesCommands
            {
                Directory = arguments[1],
                ByLabels = parameters
            };
        }

        #endregion

        #region Private Methods
        private void Run()
        {
            CommandsHelper.ForeachFiles(Directory, file => { RenameFile(file, GetNewFileName(file)); });
        }

        private void RenameFile(string file, string newFile)
        {
            var fileInfo = new FileInfo(file);
            var newFileCompletePath = $@"{fileInfo.DirectoryName}\{newFile}{fileInfo.Extension}";
            if (File.Exists(newFileCompletePath))
            {
                ConsoleHelper.Info($"{newFile} already exist.");
                return;
            }
            ConsoleHelper.Info($"{file} named as {newFile}");
            File.Move(file, newFileCompletePath);
        }

        private string GetNewFileName(string file)
        {
            string newFile =  _actionService.GetSubFolder(file, ByLabels,
                new MainFolder(string.Empty),
                new FileFormatters());
            return newFile;
        }
        #endregion 
    }
}