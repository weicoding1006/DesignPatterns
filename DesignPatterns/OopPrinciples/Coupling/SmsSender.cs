public class SmsSender : INotificationService
{
    public void SendNotification(string message)
    {
        Console.WriteLine("SMS送出" + message);
    }
}