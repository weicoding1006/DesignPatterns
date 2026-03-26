public class Subscriber : IObserver
{
    private string _name;
    public Subscriber(string name) => _name = name;
    public void Update(string videoTitle)
    {
        Console.WriteLine($"{_name}收到通知了!趕快去看:{videoTitle}");
    }
}