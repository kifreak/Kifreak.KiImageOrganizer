using Kifreak.KiImageOrganizer.CoreConsole.CommandFactory;
using Kifreak.KiImageOrganizer.CoreConsole.Commands;
using Kifreak.KiImageOrganizer.CoreConsole.Configuration;
using Kifreak.KiImageOrganizer.CoreConsole.Helpers;
using System.IO;

namespace Kifreak.KiImageOrganizer.CoreConsole.Commands
{
    public class ImageExistsCommand : ICommand, ICommandFactory
    {
        public ImageExistsCommand(ParameterParser parameterParser)
        {
            _parameterParser = parameterParser;
        }
        private Random _rmd = new Random();
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
            List<string> fileData = destinationFiles.Select(GetFileInfo).Where(t => t != null).ToList();
                
       
            ConsoleHelper.Info($"Files in destination: {destinationFiles.Count:N0} ");
            string baseDirectory = Path.Combine(Origin, "organized");
            string okFiles = Path.Combine(baseDirectory, "exist");
            string koFiles = Path.Combine(baseDirectory, "noExist");

            Directory.CreateDirectory(baseDirectory);
            Directory.CreateDirectory(okFiles);
            Directory.CreateDirectory(koFiles);
            Parallel.ForEach(originFilesInfo, file =>
            {
                bool exists = fileData.Contains($"{file.Length}:{file.Name}");
                string destiny = "";
                if (exists)
                {
                    destiny = Path.Combine(okFiles, file.Name);
                }
                else
                {
                    destiny = Path.Combine(koFiles, file.Name);
                }
                if (File.Exists(destiny))
                {
                    var newFile = new FileInfo(destiny);
                    var extension = newFile.Extension;
                    destiny = destiny.Replace(extension, $"_{GetRandom()}{extension}");

                }
                File.Copy(file.FullName, destiny);
            });
           
            return Task.CompletedTask;


        }

        string GetRandom()
        {
            return _rmd.Next(0, int.MaxValue).ToString();
        }
        string GetFileInfo(string file)
        {
            if (file == null)
            {
                return null;
            }
            try
            {
                FileInfo fileInfo = new FileInfo(file);
                return $"{fileInfo.Length}:{fileInfo.Name}";
            }catch(Exception ex)
            {
                ConsoleHelper.Error($"Error getting file: {file}");
                return null;
            }

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
