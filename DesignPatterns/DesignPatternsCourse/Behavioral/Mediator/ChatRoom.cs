public class ChatRoom : IChatRoomMediator
{
    private List<MediatorUser> _users = new List<MediatorUser>();
    public void RegisterUser(MediatorUser user)
    {
        if (!_users.Contains(user))
        {
            _users.Add(user);
            user.SetChatRoom(this);
        }
    }

    public void SendMessage(string message, MediatorUser sender)
    {
        foreach (var user in _users)
        {
            if (user != sender)
            {
                user.Receive(message, sender.Name);
            }
        }
    }

    public void SendPrivateMessage(string message, MediatorUser sender, string receiverName)
    {
        // 1. 在我們的使用者名單中，尋找名稱相符的人
        var receiver = _users.Find(user => user.Name == receiverName);

        // 2. 如果有找到那個人，就只觸發他的 Receive 方法，其他人不會收到
        if (receiver != null)
        {
            receiver.Receive($"[私訊] {message}", sender.Name);
        }
        else
        {
            // 如果沒找到，可以給個系統提示
            Console.WriteLine($"系統提示: 找不到使用者 {receiverName}");
        }
    }
}