public class BadSqlOrderRepository
{
    public void SaveOrder(string orderData)
    {
        Console.WriteLine($"儲存訂單資料:{orderData}");
    }
}

public class BadOrderService
{
    private BadSqlOrderRepository _repository;
    public BadOrderService()
    {
        // ❌ 在內部直接 new 出來，高度耦合
        _repository = new BadSqlOrderRepository();
    }

    public void ProcessOrder(string orderData)
    {
        // ... 執行驗證庫存、計算折扣等複雜的商業邏輯 ...
        // 儲存訂單
        _repository.SaveOrder(orderData);
    }
}