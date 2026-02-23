public interface IBadMachine
{
    void Print(string content);
    void Scan(string content);
    void Fax(string content);
}

public class BadAdvancedMachine : IBadMachine
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

public class BadBasicPrinter : IBadMachine
{
    public void Fax(string content)
    {
        throw new NotImplementedException("不支援");
    }

    public void Print(string content)
    {
        Console.WriteLine(content);
    }

    public void Scan(string content)
    {
        throw new NotImplementedException("不支援");
    }
}