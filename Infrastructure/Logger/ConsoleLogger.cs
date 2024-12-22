using Application.Common.Interfaces;
using Application.ConsoleWrapper;

namespace Implementation.Logger;

public class ConsoleLogger(IConsoleWrapper consoleWrapper) : ILogger
{
    public void LogInformation(string message)
    {
        consoleWrapper.WriteLine($"{DateTime.UtcNow} - {message}");
    }
}