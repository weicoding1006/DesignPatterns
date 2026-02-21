

public class EmailSender : INotificationService
{
    public void SendNotification(string message)
    {
        Console.WriteLine("寄送信件" + message);
    }
}