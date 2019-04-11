using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.CommandFactory;
using MetaDataFileInfo.Classes;

namespace Kifreak.KiImageOrganizer.Console.Commands
{
    public class OrganizerImagesCommand : ICommand, ICommandFactory
    {
        public string Directory { get; set; }
        public string[] ByLabels { get; set; }
       
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
                System.Console.WriteLine("Organizer Images Description: ");
                ConsoleHelper.Description(Description);
                return false;
            }

            if (ByLabels.Any(t => !_actions.ContainsKey(t)))
            {
                ConsoleHelper.Error("There are labels that are not accepted");
                ConsoleHelper.Description(Description);
                return false;
            }

            return true;

        }
        #endregion

        #region CommandFactory
        public string CommandName => "OrganizerImages";
        public string Description => @"Organize Images. Params Actions. Example: c:\MyFolder City Date";

        public ICommand MakeCommand(string[] arguments)
        {
            if (arguments.Length - 2 <= 0)
            {
                return new OrganizerImagesCommand();

            }
            string[] toLabels = new string[arguments.Length - 2];
            var iteration = 0;
            for (var i = 2; i < arguments.Length; i++)
            {
                toLabels[iteration] = arguments[i];
                iteration++;
            }

            var fileName = string.Empty;
            if (arguments.Length > 1)
            {
                fileName = arguments[1];
            }

            return new OrganizerImagesCommand
            {
                Directory = fileName,
                ByLabels = toLabels
            };
        }
        #endregion
        
        #region Private Methods
        private List<KeyValuePair<string, Property>> _metadataInfo;
        private SubFolders _subFolder;
        private readonly Dictionary<string, Func<SubFolders, List<KeyValuePair<string, Property>>,SubFolders>> _actions = new Dictionary<string, Func<SubFolders, List<KeyValuePair<string, Property>>, SubFolders>>
        {
            {"City" ,(folders, metadata) => new City("city",folders, metadata)},
            {"Road" ,(folders, metadata) => new City("road",folders, metadata)},
            {"Country" ,(folders, metadata) => new City("country",folders, metadata)},
            {"County" ,(folders, metadata) => new City("county",folders, metadata)},
            {"DateTime", (folders,metadata)=> new ByDate(folders, metadata)}
        };
        private void Run()
        {
            var allFiles = System.IO.Directory.GetFiles(Directory);
            foreach (string file in allFiles)
            {
                string newFolder = GetFileFolder(file);
                CreateFolderIfIsNecessary(newFolder);
                MoveFileToNewPath(file,newFolder);
            }
           
        }
        private string GetFileFolder(string file)
        {
            _subFolder = new MainFolder($@"{Directory}\Organized");
            OrganizeFile(file);
            string newFolder = _subFolder.GetSubFolder();
            System.Console.WriteLine("File: " + file + " --> " + newFolder);
            return newFolder;
        }
        
        private void OrganizeFile(string file)
        {
            GetMetadataInfo(file);
            foreach (string label in ByLabels)
            {
                _subFolder = _actions[label].Invoke(_subFolder,_metadataInfo);
            }

        }
        private void GetMetadataInfo(string file)
        {
            MetaFileInfo metaData = new MetaFileInfo(file);
            _metadataInfo = metaData.Where(t => t.Value?.Value != null).ToList();
        }

        private void CreateFolderIfIsNecessary(string newFolder)
        {
            System.IO.Directory.CreateDirectory(newFolder);
        }

        private void MoveFileToNewPath(string fileName,string newFolder)
        {
            string newFile = newFolder + "\\" + Path.GetFileName(fileName);
            if (!File.Exists(newFile))
            {
                File.Copy(fileName,newFile);
            }
        }
        #endregion
    }
}