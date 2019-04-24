using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.Console.Helpers
{
    public static class CommandsHelper
    {
        public static void ForeachFiles(string directory, Func<string, Task> action)
        {
            var allFiles = System.IO.Directory.GetFiles(directory);
            foreach (string file in allFiles)
            {
                Task tsk = action.Invoke(file);
                tsk.Wait();
            }
        }

        public static string RemoveAllNotAllowedCharacters(string path)
        {
            var regex = new Regex("<|“|\"|:|>|\\||\\\\|/|\\?|\\*");
            return regex.Replace(path, "");
        }
    }
}