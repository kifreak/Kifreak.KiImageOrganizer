using System;

namespace Kifreak.KiImageOrganizer.Console
{
    public static class ConsoleHelper
    {
        public static void Error(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"Error: {message}");
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Description(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }
    }
}