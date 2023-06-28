using System.Collections.Generic;
using Kifreak.KiImageOrganizer.CoreConsole.CommandFactory;
using Kifreak.KiImageOrganizer.CoreConsole.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.CoreConsole.Commands
{
    public class RemoveCacheCommand : ICommand, ICommandFactory
    {
        #region ICommand

        public Task Execute()
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory());
            var filesToDelete = files.Where(t => new FileInfo(t).Extension == ".log");
            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }

            return Task.CompletedTask;
        }

        public bool Validate()
        {
            return true;
        }

        #endregion ICommand

        #region ICommandFactory

        public string CommandName => "RemoveCache";
        public string Description => "Remove all cache files";
        public Dictionary<string, string> OptionsDescription { get; }

        public ICommand MakeCommand(string[] arguments)
        {
            return Config.Get<RemoveCacheCommand>();
        }

        #endregion ICommandFactory
    }
}