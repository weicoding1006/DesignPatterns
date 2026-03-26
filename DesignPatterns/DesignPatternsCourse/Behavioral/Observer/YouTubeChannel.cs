public class YouTubeChannel : ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    private string _latestVideoTitle;

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);

    public void Notify()
    {
        foreach(var observer in _observers)
        {
            observer.Update(_latestVideoTitle);
        }
    }

    public void UploadVideo(string title)
    {
        Console.WriteLine($"頻道發布了新影片:{title}");
        _latestVideoTitle = title;
        Notify();
    }
}