using System;
using System.Text.RegularExpressions;

namespace Kifreak.KiImageOrganizer.Console.Helpers
{
    public static class CommandsHelper
    {
        public static void ForeachFiles(string directory,Action<string> action)
        {
            var allFiles = System.IO.Directory.GetFiles(directory);
            foreach (string file in allFiles)
            {
               action.Invoke(file);
            }
        }

        public static string RemoveAllNotAllowedCharacters(string path)
        {
            var regex = new Regex("<|“|\"|:|>|\\||\\\\|/|\\?|\\*");
            return regex.Replace(path, "");
        }
    }
}