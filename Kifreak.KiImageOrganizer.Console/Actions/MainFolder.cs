using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Formatters;

namespace Kifreak.KiImageOrganizer.Console.Actions
{
    public class MainFolder : SubFolders
    {
        private readonly string _path;


        public MainFolder(string path)
        {
            _path = path;
        }

        public override async Task<string> GetSubFolder(IFormatter formatter)
        {
            return formatter.Format(_path,string.Empty);
        }
    }
}