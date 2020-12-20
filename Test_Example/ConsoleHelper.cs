using System;

namespace Test_Example
{
    public static class ConsoleHelper
    {
        public static void UpdateColor(this ConsoleColor consoleColor) => Console.ForegroundColor = consoleColor;

        public static void ResetColor() => Console.ResetColor();

        public static void WriteLineWithCurrentColor(this ConsoleColor consoleColor, string message)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(message);
            ResetColor();
        }
    }
}
