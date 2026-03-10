public class BadCheckoutService
{
    public void Pay(string paymentMethod, decimal amount)
    {
        if (paymentMethod == "CreditCard")
        {
            Console.WriteLine($"使用信用卡付款 {amount} 元");
            // 複雜的信用卡連接邏輯...
        }
        else if (paymentMethod == "LinePay")
        {
            Console.WriteLine($"使用 LINE Pay 付款 {amount} 元");
            // 複雜的 LinePay 連接邏輯...
        }
        else if (paymentMethod == "ApplePay")
        {
            // ...
        }
    }
}