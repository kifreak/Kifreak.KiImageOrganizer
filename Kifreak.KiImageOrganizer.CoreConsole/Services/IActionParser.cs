using Kifreak.KiImageOrganizer.CoreConsole.Actions;
using Kifreak.KiImageOrganizer.CoreConsole.Models;

namespace Kifreak.KiImageOrganizer.CoreConsole.Services
{
    public interface IActionParser
    {
        bool HasNoExistAction(string[] actions);

        SubFolders InvokeWithAlternative(KeysAlternatives keysAlternatives, SubFolders subFolders,
            string filePath);
    }
}