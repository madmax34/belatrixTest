using System;
using BelatrixTest.Logger.Interfaces;

namespace BelatrixTest.Logger
{
    public class ConsoleWriter : IConsoleWriter
    {
        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void SetForegroundColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
    }
}
