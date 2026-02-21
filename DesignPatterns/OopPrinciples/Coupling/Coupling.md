# 耦合 (Coupling)

## 什麼是耦合？

**耦合**是指模組（類別）之間的**依賴程度**。

- **高耦合 (Tight Coupling)**：類別之間直接依賴具體實作，修改一個類別會連帶影響其他類別。
- **低耦合 (Loose Coupling)**：類別之間透過**介面 (Interface)** 溝通，彼此不認識對方的實作細節，修改時不會互相影響。

> 好的程式設計應該追求 **低耦合**，讓程式更容易擴充和維護。

---

## 程式碼範例

### 1. 定義介面 — `INotificationService.cs`

首先定義一個通知服務的**介面**，規範所有通知方式都必須實作 `SendNotification` 方法：

```csharp
public interface INotificationService
{
    void SendNotification(string message);
}
```

> 介面就像是一份「合約」，規定了要提供哪些功能，但不管怎麼實作。

---

### 2. 實作介面 — `EmailSender.cs` / `SmsSender.cs`

不同的通知方式各自實作介面：

**Email 通知：**

```csharp
public class EmailSender : INotificationService
{
    public void SendNotification(string message)
    {
        Console.WriteLine("寄送信件" + message);
    }
}
```

**SMS 簡訊通知：**

```csharp
public class SmsSender : INotificationService
{
    public void SendNotification(string message)
    {
        Console.WriteLine("SMS送出" + message);
    }
}
```

> 兩個類別都實作了 `INotificationService`，所以可以**互相替換**使用。

---

### 3. 使用介面而非具體類別 — `Order.cs`

`Order` 類別只依賴 `INotificationService` 介面，**不知道**實際用的是 Email 還是 SMS：

```csharp
public class Order
{
    private readonly INotificationService NotificationService;

    public Order(INotificationService notificationService)
    {
        NotificationService = notificationService;
    }

    public void PlaceOrder()
    {
        NotificationService.SendNotification("訂單送出");
    }
}
```

> 這就是 **低耦合** 的關鍵：`Order` 不依賴任何具體的通知類別，只依賴介面。

---

### 4. 實際使用 — `Program.cs`

在執行時，透過**建構子注入 (Constructor Injection)** 決定要用哪種通知方式：

```csharp
// 使用 SMS 通知
Order order = new Order(new SmsSender());
order.PlaceOrder(); // 輸出：SMS送出訂單送出

// 想改用 Email？只需換一行：
// Order order = new Order(new EmailSender());
```

> 切換通知方式時，**完全不需要修改 `Order` 的程式碼**，只要換掉傳入的實作即可。

---

## 高耦合 vs 低耦合 比較

| | 高耦合 ❌ | 低耦合 ✅（本範例） |
|---|---|---|
| **依賴對象** | 直接依賴具體類別 | 依賴介面 |
| **新增通知方式** | 需修改 `Order` 類別 | 只需新增一個實作介面的類別 |
| **可測試性** | 難以單元測試 | 可用 Mock 物件測試 |
| **維護成本** | 高 | 低 |

---

## 重點整理

1. **介面 (Interface)** 是降低耦合的關鍵工具
2. **依賴注入 (Dependency Injection)**：透過建構子傳入依賴，而非在類別內部 `new` 出來
3. **開放封閉原則 (OCP)**：對擴充開放、對修改封閉 — 新增通知方式不需改動現有程式碼
4. 低耦合讓程式更**易擴充、易測試、易維護**
