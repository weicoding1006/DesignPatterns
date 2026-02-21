

public class Order
{
    private readonly INotificationService NotificationService;

    public Order(INotificationService notificationService){
        NotificationService = notificationService;
    }
    public void PlaceOrder()
    {
        NotificationService.SendNotification("訂單送出");
    }
}