public interface IOrderRepository
{
    void SaveOrder(string orderData);
}

public class SqlOrderRepository : IOrderRepository
{
    public void SaveOrder(string orderData)
    {
        Console.WriteLine($"[SQL Server] 儲存訂單資料: {orderData}");
    }
}

public class MongoDbOrderRepostitory : IOrderRepository
{
    public void SaveOrder(string orderData)
    {
        Console.WriteLine($"[MongoDB] 儲存訂單資料: {orderData}");
    }
}

public class OrderService
{
    private IOrderRepository _repository;
    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public void ProcessOrder(string orderData)
    {
        _repository.SaveOrder(orderData);
    }
}