using System;

namespace Kifreak.KiImageOrganizer.Console.Helpers
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
            WriteLineDarkYellow(message);
        }

        public static void EndProgram()
        {
            WriteLineDarkGreen("Program finish. Press any key to close this window.");
            System.Console.ReadKey();
        }

        public static void WriteLineDarkYellow(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteDarkYellow(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Console.Write(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteLineDarkGreen(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteLineDarkBlue(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteLineSeparator()
        {
            System.Console.WriteLine("==========================================================================================");
        }

        public static void JumpLine(int numberOfLinesToJump)
        {
            for (var i = 0; i < numberOfLinesToJump; i++)
            {
                System.Console.WriteLine();
            }
        }
    }
}