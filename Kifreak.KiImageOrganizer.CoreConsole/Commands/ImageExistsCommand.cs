using Kifreak.KiImageOrganizer.CoreConsole.CommandFactory;
using Kifreak.KiImageOrganizer.CoreConsole.Commands;
using Kifreak.KiImageOrganizer.CoreConsole.Configuration;
using Kifreak.KiImageOrganizer.CoreConsole.Helpers;

namespace Kifreak.KiImageOrganizer.CoreConsole.Commands
{
    public class ImageExistsCommand : ICommand, ICommandFactory
    {
        public ImageExistsCommand(ParameterParser parameterParser)
        {
            _parameterParser = parameterParser;
        }

        private readonly ParameterParser _parameterParser;
        public string Origin { get; set; }
            
        public string Destination { get; set; }


        string ICommandFactory.CommandName => "Exists";

        string ICommandFactory.Description => "Review all files in a folder and check if exists in another folder";

        Dictionary<string, string> ICommandFactory.OptionsDescription => new Dictionary<string, string>
        {
            {"origin", "Folder to check"},
                { "destination", "Folder to compare"}
        };

        public Task Execute()
        {
            ConsoleHelper.Info("Loading files in origin");
            List<string> originFiles = CommandsHelper.GetFilesInaFolder(Origin);
            List<FileInfo> originFilesInfo = originFiles.Select(t => new FileInfo(t)).ToList();
            ConsoleHelper.Info($"Files in origin: {originFilesInfo.Count} ");
            ConsoleHelper.Info("Loading files in destination");
            List<string> destinationFiles = CommandsHelper.GetFilesInaFolder(Destination);
            List<FileInfo> destinationFilesInfo = destinationFiles.Select(t => new FileInfo(t)).ToList();
            ConsoleHelper.Info($"Files in destination: {destinationFilesInfo.Count} ");
            string baseDirectory = Path.Combine(Origin, "organized");
            string okFiles = Path.Combine(baseDirectory, "exist");
            string koFiles = Path.Combine(baseDirectory, "noExist");

            Directory.CreateDirectory(baseDirectory);
            Directory.CreateDirectory(okFiles);
            Directory.CreateDirectory(koFiles);


            foreach (FileInfo file in originFilesInfo)
            {
                bool exists = destinationFilesInfo.Any(d => file.Length == d.Length && d.Name == file.Name);
                if (exists)
                {
                    File.Copy(file.FullName, Path.Combine(okFiles,file.Name));
                } else
                {
                    File.Copy(file.FullName, Path.Combine(koFiles, file.Name));
                }
            }
            return Task.CompletedTask;
            

        }

        public bool Validate()
        {
            return Directory.Exists(Origin) && Directory.Exists(Destination);
        }

        CommandFactory.ICommand ICommandFactory.MakeCommand(string[] arguments)
        {
            string[] parameters = _parameterParser.GetParameters(arguments, 1, 2);
            var imageExecCommand = Config.Get<ImageExistsCommand>();
            if (parameters == null)
            {
                return imageExecCommand;
            }
            imageExecCommand.Origin = parameters[0];
            imageExecCommand.Destination = parameters[1];
            return imageExecCommand;



        }
    }
}
