# 什麼是 Open-Closed Principle (開閉原則)？

**開閉原則 (Open/Closed Principle, OCP)** 是 SOLID 原則中的 'O'。
它的核心概念是：**「軟體實體（類別、模組、函式等）應該對擴充開放，對修改封閉。」**
(Software entities should be open for extension, but closed for modification.)

簡單來說，當我們需要為系統新增功能時，我們應該透過**「新增程式碼」**（擴充）來達成，而不是去**「修改既有的程式碼」**。這樣可以減少引入新 Bug 的風險，並提高程式碼的可維護性。

---

## ❌ 違反 OCP 的壞例子

在此資料夾中，`BadDiscountCalculator.cs` 就是一個違反 OCP 的典型例子：

```csharp
public class BadDiscountCalculator
{
    public double CalculateDiscount(string customerType, double amount)
    {
        if(customerType == "Regular")
        {
            return amount * 0.05;
        }
        else if(customerType == "Gold")
        {
            return amount * 0.10;
        }
        return 0;
    }
}
```

**為什麼這設計不好？**
如果今天我們想要新增一種「VIP 會員」(VIP) 的折扣，我們就**必須修改** `BadDiscountCalculator` 類別，在裡面加上一個 `else if(customerType == "VIP")` 的判斷條件。這違反了「對修改封閉」的原則。每新增一種折扣情形，就要修改一次核心邏輯，如果判斷邏輯變得複雜，非常容易改壞原本就正常運作的程式碼。

---

## ✅ 符合 OCP 的好例子 (使用策略模式 Strategy Pattern)

為了遵守 OCP，我們可以利用**介面 (Interface)**（或者抽象類別）來將變化隔離出來。你在此目錄中實作了「策略模式」來達成這個目的：

### 1. 定義抽象介面
把會變動的「折扣計算邏輯」抽象成一個介面 `IDiscountStrategy.cs`：

```csharp
public interface IDiscountStrategy
{
    double CalculateDiscount(double amount);
}
```

### 2. 針對不同情境實作 (對擴充開放)
建立各自獨立的類別實作折扣邏輯，例如 `RegularDiscount.cs` 和 `GoldDiscount.cs`：

```csharp
// 一般會員折扣 (RegularDiscount.cs)
public class RegularDiscount : IDiscountStrategy
{
    public double CalculateDiscount(double amount)
    {
        return amount * 0.05;
    }
}

// 金卡會員折扣 (GoldDiscount.cs)
public class GoldDiscount : IDiscountStrategy
{
    public double CalculateDiscount(double amount)
    {
        return amount * 0.10;
    }
}
```

### 3. 主程式不需因新情境被修改 (對修改封閉)
在 `DiscountCalculator.cs` 中，我們只依賴介面 `IDiscountStrategy`，不去管實際上是哪一種折扣：

```csharp
public class DiscountCalculator
{
    // 依賴於介面，不依賴於具體實作
    public double Calculate(IDiscountStrategy discountStrategy, double amount)
    {
        return discountStrategy.CalculateDiscount(amount);
    }
}
```

### 總結好處：
如果未來要新增「VIP 會員折扣」，我們**完全不需要修改** `DiscountCalculator`，也不用動到現有的 `RegularDiscount` 或 `GoldDiscount`，我們只需要寫一個**全新**的 `VipDiscount` 類別去實作 `IDiscountStrategy` 介面，然後傳入 `DiscountCalculator` 即可。

這完美達成了**「對擴充開放（能新增 VipDiscount 新類別），對修改封閉（不用更改 DiscountCalculator 原有邏輯）」**的目標！
