using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.CommandFactory;
using Kifreak.KiImageOrganizer.Console.Configuration;

namespace Kifreak.KiImageOrganizer.Console.Commands
{
    public class RemoveCacheCommand:ICommand, ICommandFactory
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
        #endregion

        #region ICommandFactory

        public string CommandName => "RemoveCache";
        public string Description => "Remove all cache files";
        public ICommand MakeCommand(string[] arguments)
        {
            return Config.Get<RemoveCacheCommand>();
        }
        #endregion
    }
}