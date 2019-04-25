using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.Formatters;

namespace Kifreak.KiImageOrganizer.Console.Services
{
    public interface IActionService
    {
        string ActionsToString();

        bool HasNoExistAction(string[] actions);

        Task<string> GetSubFolder(string file, string[] labels, SubFolders subFolder, IFormatter formatter);
    }
}