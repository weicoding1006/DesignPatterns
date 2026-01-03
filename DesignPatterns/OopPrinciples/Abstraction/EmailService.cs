public class EmailService
{
    public void SendEmail()
    {
        Connect();
        Authenticate();
        Disconnect();
    }

    private void Connect()
    {
        Console.WriteLine("Connecting to email server");
    }

    private void Authenticate()
    {
        Console.WriteLine("Authenticating");
    }

    private void Disconnect()
    {
        Console.WriteLine("Disconnecting from email server");
    }
}