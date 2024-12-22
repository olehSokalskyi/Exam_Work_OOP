using Application.Common.Interfaces;
using Application.ConsoleWrapper;
using Microsoft.Extensions.Configuration;

namespace Implementation.Logger;

public static class ConfigurationKeys
{
    public const string LoggingLogType = "Logging:LogType";
    public const string LoggingLogPath = "Logging:LogPath";
}

public class LoggerFactory : ILoggerFactory
{
    private readonly IConfiguration _configuration;
    private readonly IConsoleWrapper _consoleWrapper;

    public LoggerFactory(IConfiguration configuration, IConsoleWrapper consoleWrapper)
    {
        _configuration = configuration;
        _consoleWrapper = consoleWrapper;
    }

    public ILogger CreateLogger()
    {
        var loggerType = _configuration[ConfigurationKeys.LoggingLogType];
        var filePath = _configuration[ConfigurationKeys.LoggingLogPath];

        return loggerType switch
        {
            "Console" => new ConsoleLogger(_consoleWrapper),
            "File" => new FileLogger(filePath),
            _ => throw new ArgumentException("Invalid logger type")
        };
    }
}