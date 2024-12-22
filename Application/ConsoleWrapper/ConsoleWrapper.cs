namespace Application.ConsoleWrapper;

public class ConsoleWrapper : IConsoleWrapper
{
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
    
    public string ReadLine()
    {
        return Console.ReadLine();
    }
}