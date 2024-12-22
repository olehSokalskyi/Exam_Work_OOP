namespace Application.ConsoleWrapper;

public interface IConsoleWrapper
{
    void WriteLine(string message);
    string ReadLine();
}