using System;

namespace Kifreak.KiImageOrganizer.CoreConsole.Helpers
{
    public static class ConsoleHelper
    {
        public static void Error(string message)
        {
            WriteLineInColor($"Error: {message}", ConsoleColor.Red);
        }

        public static void Info(string message)
        {
            WriteLineDarkYellow(message);
        }

        public static void EndProgram()
        {
            WriteLineDarkGreen("Program finish.");
        }

        public static void WriteLineDarkYellow(string message)
        {
            WriteLineInColor(message, ConsoleColor.DarkYellow);
        }

        public static void WriteDarkYellow(string message)
        {
            WriteInColor(message, ConsoleColor.DarkYellow);
        }

        public static void WriteLineDarkGreen(string message)
        {
            WriteLineInColor(message, ConsoleColor.DarkGreen);
        }

        public static void WriteLineDarkBlue(string message)
        {
            WriteLineInColor(message, ConsoleColor.DarkBlue);
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

        public static void WriteLineInColor(string message, ConsoleColor color)
        {
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteInColor(string message, ConsoleColor color)
        {
            System.Console.ForegroundColor = color;
            System.Console.Write(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }
    }
}