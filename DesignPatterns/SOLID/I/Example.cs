public interface IPrinter
{
    void Print(string content);
}

public interface IScanner
{
    void Scan(string content);
}

public interface IFaxer
{
    void Fax(string content);
}

public class AdvancedMachine : IPrinter, IScanner, IFaxer
{
    public void Fax(string content)
    {
        Console.WriteLine(content);
    }

    public void Print(string content)
    {
        Console.WriteLine(content);
    }

    public void Scan(string content)
    {
        Console.WriteLine(content);
    }
}

public class BasicPrinter : IPrinter
{
    public void Print(string content)
    {
        Console.WriteLine(content);
    }
}