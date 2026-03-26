

public interface IChatRoomMediator
{
    void SendMessage(string message, MediatorUser sender);
    void RegisterUser(MediatorUser user);
    void SendPrivateMessage(string message, MediatorUser sender, string receiverName);
}