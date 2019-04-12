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

        public static void Info(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        public static void EndProgram()
        {
            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
            System.Console.WriteLine("Program finish. Press any key to close this window.");
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.ReadKey();
        }
    }

}