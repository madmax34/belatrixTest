using System;

namespace BelatrixTest.Logger.Interfaces
{
    public interface IConsoleWriter
    {
        void Write(string format, params object[] args);
        void WriteLine(string format, params object[] args);
        void SetForegroundColor(ConsoleColor color);
    }
}
