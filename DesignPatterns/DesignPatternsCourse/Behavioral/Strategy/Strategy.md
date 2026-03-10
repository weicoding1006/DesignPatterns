# 策略模式 (Strategy Pattern)

## 1. 概念介紹
策略模式是一種**行為型**設計模式。它能將一系列的「演算法或行為」各自封裝成獨立的類別，並且可以讓它們互相替換。
該模式讓演算法的變化獨立於使用演算法的客戶端。它同時也是實踐 **SOLID 原則中「開閉原則 (Open/Closed Principle, OCP)」** 的經典模式，能有效消滅程式碼中過多的 `if-else` 或 `switch` 判斷。

## 2. 解決的問題
在沒有使用設計模式的情況下，我們可能會將所有的邏輯塞在同一個類別的同一個方法中，並透過 `if-else` 來切換不同的行為。

例如在 `BadCheckoutService.cs` 中，我們將所有的結帳方式寫在一起：

```csharp
// BadCheckoutService.cs
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
```

這個寫法的缺點非常明顯：
1. **違反開閉原則 (OCP)**：每次新增一種付款方式，你都必須修改 `BadCheckoutService` 這個類別。
2. **類別膨脹與高耦合**：所有的結帳行為細節都塞在同一個服務裡，導致類別過於龐大且難以維護。

## 3. 策略模式重構
策略模式透過三個核心角色來解決上述問題：

1. **Strategy (策略介面)**：定義所有支援演算法的共通介面 (`IPaymentStrategy`)。
2. **Concrete Strategy (具體策略)**：實作介面的具體類別 (`CreditCardStrategy`, `LinePayStrategy`)。
3. **Context (上下文)**：維護一個策略物件的參考，負責呼叫策略物件 (`CheckoutContext`)。

### 步驟一：定義策略介面
把付款行為抽離出一個介面：
```csharp
// IPaymentStrategy.cs
public interface IPaymentStrategy
{
    void Pay(decimal amount);
}
```

### 步驟二：實作具體策略
將每一種付款方式獨立成自己的類別，各自完成自己的結帳邏輯：
```csharp
// ConcreteStrategy.cs
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
```

### 步驟三：建立上下文
將結帳服務改為 Context。它不再關心「怎麼付款」，而是直接將工作委派給當下綁定的策略：
```csharp
// CheckoutContext.cs
public class CheckoutContext
{
    private IPaymentStrategy _paymentStrategy;
    
    // 建構子注入初始策略
    public CheckoutContext(IPaymentStrategy paymentStrategy)
    {
        _paymentStrategy = paymentStrategy;
    }

    // 允許在執行期間動態切換策略
    public void SetPaymentStrategy(IPaymentStrategy paymentStrategy)
    {
        _paymentStrategy = paymentStrategy;
    }

    public void ProcessCheckout(decimal amount)
    {
        // 這裡不需要任何 if-else 判斷！
        _paymentStrategy.Pay(amount);
    }
}
```

### 步驟四：客戶端的使用方式
在 `Program.cs` 裡，客戶端可以根據需要自由選擇或切換策略：
```csharp
// Program.cs
decimal orderAmount = 1000m;

// 1. 客戶選擇信用卡付款
IPaymentStrategy creditStrategy = new CreditCardStrategy();
CheckoutContext checkout = new CheckoutContext(creditStrategy);
checkout.ProcessCheckout(orderAmount);

// 2. 結帳到一半，客戶反悔說想改用 LINE Pay，可動態切換策略！
checkout.SetPaymentStrategy(new LinePayStrategy());
checkout.ProcessCheckout(orderAmount);
```

## 4. 總結
**優點：**
- **符合開閉原則 (OCP)**：未來若要增加「Apple Pay」，只需新增一個 `ApplePayStrategy` 類別並實作 `IPaymentStrategy`，不需修改既有的 `CheckoutContext` 代碼。
- **避免多重條件判斷**：消滅了原本落落長的 `if-else` 或 `switch` 語法。
- **動態替換行為**：可以在程式執行期間 (Runtime) 動態變更物件的行為。

**適用情境：**
當你發現一段程式碼包含許多 `if-else` 作為切換演算法、邏輯行為或業務規則的判斷時，且這些行為的目的性一致，非常推薦將其重構為 **策略模式**。
