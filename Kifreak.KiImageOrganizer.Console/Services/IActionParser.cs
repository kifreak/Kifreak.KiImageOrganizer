using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.Models;

namespace Kifreak.KiImageOrganizer.Console.Services
{
    public interface IActionParser
    {
        bool HasNoExistAction(string[] actions);

        SubFolders InvokeWithAlternative(KeysAlternatives keysAlternatives, SubFolders subFolders,
            string filePath);
    }
}