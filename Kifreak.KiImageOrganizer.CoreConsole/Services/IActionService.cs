using System.Threading.Tasks;
using Kifreak.KiImageOrganizer.CoreConsole.Actions;
using Kifreak.KiImageOrganizer.CoreConsole.Formatters;

namespace Kifreak.KiImageOrganizer.CoreConsole.Services
{
    public interface IActionService
    {
        string ActionsToString();

        bool HasNoExistAction(string[] actions);

        Task<string> GetSubFolder(string file, string[] labels, SubFolders subFolder, IFormatter formatter);
    }
}