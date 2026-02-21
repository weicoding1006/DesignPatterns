public class UserService
{
    public void Register(User user)
    {
        SOLIDEmailSender emailSender = new SOLIDEmailSender();
        emailSender.SendEmail(user.Email, "註冊");
    }
}