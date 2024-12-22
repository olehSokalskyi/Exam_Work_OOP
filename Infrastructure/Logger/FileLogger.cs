using Application.Common.Interfaces;

namespace Implementation.Logger;

public class FileLogger(string path) : ILogger
{
    public void LogInformation(string message)
    {
        File.AppendAllText(path, $"{DateTime.UtcNow} - {message}");
    }
}