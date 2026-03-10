public class CreditCardStrategy : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"連線銀行 API... 使用 [信用卡] 成功付款 {amount} 元");
    }
}

public class LinePayStrategy : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
       Console.WriteLine($"開啟 LINE App... 使用 [LINE Pay] 成功付款 {amount} 元");
    }
}