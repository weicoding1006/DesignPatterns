# 依賴反轉原則 (Dependency Inversion Principle, DIP)

**SOLID** 原則中的 **D** 代表 **依賴反轉原則 (Dependency Inversion Principle)**。

**核心概念**：
1. **高階模組不應該依賴於低階模組。兩者都應該依賴抽象概念。** (High-level modules should not depend on low-level modules. Both should depend on abstractions.)
2. **抽象不應該依賴細節。細節應該依賴抽象。** (Abstractions should not depend on details. Details should depend on abstractions.)

簡單來說，我們應該針對「介面 (Interface) 或抽象類別 (Abstract Class)」寫程式，而不是針對「具體實作」寫程式。這通常會搭配**依賴注入 (Dependency Injection)** 的技巧來達成。

---

## 反面教材：違反 DIP 的設計 (`BadExample.cs`)

在 `BadExample.cs` 中，我們看到一個高度耦合的設計：

```csharp
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
        // ❌ 致命錯誤：在內部直接 new 出來，高度耦合
        _repository = new BadSqlOrderRepository();
    }

    public void ProcessOrder(string orderData)
    {
        _repository.SaveOrder(orderData);
    }
}
```

**違反原因分析：**
高層次的商業邏輯模組 (`BadOrderService`) 直接依賴了低層次的資料存取模組 (`BadSqlOrderRepository`)。
* **高耦合度**：因為程式碼裡寫死了 `new BadSqlOrderRepository()`，如果今天老闆說：「我們要把資料庫從 SQL Server 換成 MongoDB」，你必須**修改** `BadOrderService` 內部的程式碼，這也同時違反了開放封閉原則 (OCP)。
* **難以進行單元測試**：在測試 `ProcessOrder` 的邏輯時，一定會連動操作到真正的資料庫，無法輕易地替換為模擬的 (Mock) Repository。

---

## 正確示範：符合 DIP 的設計 (`Example.cs`)

為了解決這個問題，我們可以引入一個抽象介面 `IOrderRepository`。

```csharp
public interface IOrderRepository
{
    void SaveOrder(string orderData);
}
```

接著讓具體的 Repository 去實作這個介面：

```csharp
// 實作一：SQL Server
public class SqlOrderRepository : IOrderRepository
{
    public void SaveOrder(string orderData)
    {
        Console.WriteLine($"[SQL Server] 儲存訂單資料: {orderData}");
    }
}

// 實作二：MongoDB
public class MongoDbOrderRepostitory : IOrderRepository
{
    public void SaveOrder(string orderData)
    {
        Console.WriteLine($"[MongoDB] 儲存訂單資料: {orderData}");
    }
}
```

最後，重構我們的 OrderService，**將相依的物件透過建構子「注入」進來 (Constructor Injection)**：

```csharp
public class OrderService
{
    private IOrderRepository _repository; // ✅ 依賴抽象介面，而非具體實作
    
    public OrderService(IOrderRepository repository)
    {
        _repository = repository; // 由外部決定要傳遞什麼 repository 進來
    }

    public void ProcessOrder(string orderData)
    {
        _repository.SaveOrder(orderData);
    }
}
```

### 實際使用的優勢 (`Program.cs`)

現在我們可以在程式的進入點動態決定要使用哪一種資料庫儲存方式，而不需要去修改 `OrderService`：

```csharp
// 如果要用 SQL Server：
OrderService orderService = new OrderService(new SqlOrderRepository());
orderService.ProcessOrder("測試");

// 如果要換成 MongoDB，只要改變外部傳入的參數即可：
OrderService orderService2 = new OrderService(new MongoDbOrderRepostitory());
orderService2.ProcessOrder("123");
```

## 總結

**依賴反轉原則 (DIP)** 的主要好處：
1. **解耦**：模組之間不再互相綁定，更容易抽換與擴充不同功能的實作。
2. **方便測試**：要對 Service 層寫單元測試時，可以輕鬆地傳入一個假的 (Mock) `IOrderRepository` 實作。
3. **分工合作**：只要 `IOrderRepository` 介面定義好了，寫商業邏輯的人與寫資料存取的人可以同時平行開發，不需互相等待。
