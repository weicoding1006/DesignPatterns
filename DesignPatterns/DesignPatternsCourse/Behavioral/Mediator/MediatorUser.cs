public class MediatorUser
{
    public string Name { get; private set; }
    private IChatRoomMediator _chatRoom; //元件只需要認識中介者

    public MediatorUser(string name)
    {
        Name = name;
    }

    public void SetChatRoom(IChatRoomMediator chatRoom)
    {
        _chatRoom = chatRoom;
    }

    public void Send(string message)
    {
        Console.WriteLine($"{Name} 發送的訊息 : {message}");
        _chatRoom.SendMessage(message, this);
    }

    public void SendPrivate(string message,string receiverName)
    {
        _chatRoom.SendPrivateMessage(message,this,receiverName);
    }

    public void Receive(string message, string senderName)
    {
        Console.WriteLine($"{Name}收到來自{senderName}的訊息:{message}");
    }
}