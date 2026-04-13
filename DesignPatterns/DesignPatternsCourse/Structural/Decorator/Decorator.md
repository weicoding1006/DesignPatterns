# Decorator 裝飾者模式

> **分類**：結構型模式（Structural Pattern）  
> **別名**：Wrapper（包裝器）

---

## 🎯 核心概念

**Decorator** 的目標是：**在不修改原始類別的情況下，動態地為物件新增功能**。

當需要對物件擴充功能，但又不想用繼承（避免類別爆炸），就可以用裝飾者層層「包裹」物件，每層包裹都能疊加新功能，且彼此獨立、可任意組合。

> 就像手搖飲料店：一杯基底茶（紅茶/綠茶）可以加珍珠、加牛奶、  
> 甚至雙倍珍珠，每種配料都是獨立的「裝飾」，可以自由堆疊。

---

## 📁 目錄結構

```
Decorator/
├── IBeverage.cs              # Component 介面
├── CondimentDecorator.cs     # Decorator 抽象基類
├── Beverages/                # Concrete Components（基底飲料）
│   ├── BlackTea.cs           #   紅茶 $30
│   └── GreenTea.cs           #   綠茶 $25
└── Decorators/               # Concrete Decorators（配料）
    ├── Boba.cs               #   珍珠 +$10
    └── Milk.cs               #   牛奶 +$15
```

---

## 🗺 UML 類別圖

```
          ┌─────────────────┐
          │   «interface»   │
          │   IBeverage     │
          │─────────────────│
          │ GetDescription()│
          │ GetCost()       │
          └────────┬────────┘
                   │ implements
       ┌───────────┴──────────────┐
       │                          │
┌──────┴──────┐      ┌────────────┴──────────────┐
│  BlackTea   │      │  CondimentDecorator        │ ← abstract Decorator
│  GreenTea   │      │───────────────────────────│
│  (Beverages)│      │ # _beverage: IBeverage     │
└─────────────┘      │───────────────────────────│
                     │ + GetDescription()         │
                     │ + GetCost()                │
                     └──────────┬────────────────┘
                                │ extends
                    ┌───────────┴───────────┐
                    │                       │
              ┌─────┴──┐             ┌──────┴──┐
              │  Boba   │             │  Milk   │
              │(+$10)   │             │ (+$15)  │
              └─────────┘             └─────────┘
                    (Decorators/)
```

---

## 📂 程式碼解析

### 1. `IBeverage.cs` — Component（元件介面）

```csharp
public interface IBeverage
{
    string GetDescription();
    double GetCost();
}
```

> 所有飲料（基底 + 裝飾者）都實作此介面，確保可以互相包裹。

---

### 2. `Beverages/BlackTea.cs`、`GreenTea.cs` — Concrete Component（基底飲料）

```csharp
public class BlackTea : IBeverage
{
    public string GetDescription() => "紅茶";
    public double GetCost()        => 30.0;
}

public class GreenTea : IBeverage
{
    public string GetDescription() => "綠茶";
    public double GetCost()        => 25.0;
}
```

> 這是「被裝飾的對象」，本身不含任何配料邏輯。

---

### 3. `CondimentDecorator.cs` — Decorator（抽象裝飾者基類）

```csharp
public abstract class CondimentDecorator : IBeverage
{
    protected IBeverage _beverage;  // 持有被包裹的物件

    public CondimentDecorator(IBeverage beverage)
    {
        _beverage = beverage;
    }

    public virtual string GetDescription() => _beverage.GetDescription();
    public virtual double GetCost()        => _beverage.GetCost();
}
```

> **關鍵設計**：同時繼承 `IBeverage` 且持有一個 `IBeverage`，  
> 讓裝飾者可以「包裹任何飲料，甚至包裹另一個裝飾者」。

---

### 4. `Decorators/Boba.cs`、`Milk.cs` — Concrete Decorator（具體配料）

```csharp
public class Boba : CondimentDecorator
{
    public Boba(IBeverage beverage) : base(beverage) { }

    public override string GetDescription() => _beverage.GetDescription() + " +珍珠";
    public override double GetCost()        => _beverage.GetCost() + 10.0;
}

public class Milk : CondimentDecorator
{
    public Milk(IBeverage beverage) : base(beverage) { }

    public override string GetDescription() => _beverage.GetDescription() + " +牛奶";
    public override double GetCost()        => _beverage.GetCost() + 15.0;
}
```

> 每個裝飾者只負責「自己加了什麼」，透過呼叫 `_beverage` 串接前面所有層次。

---

## 🧪 使用範例（Program.cs）

```csharp
// 1. 基本紅茶
IBeverage myDrink = new BlackTea();
Console.WriteLine($"{myDrink.GetDescription()} = ${myDrink.GetCost()}");
// 輸出：紅茶 = $30

// 2. 加牛奶（包一層裝飾）
myDrink = new Milk(myDrink);
Console.WriteLine($"{myDrink.GetDescription()} = ${myDrink.GetCost()}");
// 輸出：紅茶 +牛奶 = $45

// 3. 再加珍珠（再包一層裝飾）
myDrink = new Boba(myDrink);
Console.WriteLine($"{myDrink.GetDescription()} = ${myDrink.GetCost()}");
// 輸出：紅茶 +牛奶 +珍珠 = $55

// 4. 瘋狂客製化：雙份珍珠綠茶（一次套多層）
IBeverage crazyDrink = new Boba(new Boba(new GreenTea()));
Console.WriteLine($"{crazyDrink.GetDescription()} = ${crazyDrink.GetCost()}");
// 輸出：綠茶 +珍珠 +珍珠 = $45
```

### 裝飾層層包裹的呼叫鏈

```
Boba.GetCost()
  └─► Milk.GetCost()
        └─► BlackTea.GetCost()  →  30
      +15  →  45
  +10  →  55
```

---

## ⚖️ 與繼承的對比

| | 繼承 | Decorator |
|---|---|---|
| **加珍珠紅茶** | 建一個 `BlackTeaWithBoba` 類別 | `new Boba(new BlackTea())` |
| **加牛奶珍珠紅茶** | 建一個 `BlackTeaWithMilkAndBoba` 類別 | `new Boba(new Milk(new BlackTea()))` |
| **雙倍珍珠** | 建一個 `BlackTeaWithDoubleBoba` 類別 | `new Boba(new Boba(new BlackTea()))` |
| **類別數量** | 組合數爆炸 💥 | 固定幾個類別，任意組合 ✅ |

---

## ⚖️ 與其他模式比較

| 模式 | 目的 | 差異 |
|---|---|---|
| **Decorator** | 動態疊加功能 | 包裹同一介面，可多層堆疊 |
| **Proxy** | 控制存取 / 延遲初始化 | 通常只包一層，著重控制 |
| **Facade** | 簡化複雜子系統 | 提供簡單入口，不疊層 |
| **Composite** | 樹狀結構組合物件 | 著重整體與部分的關係 |

---

## ✅ 使用時機

**應該用 Decorator 的情況：**
- 需要在執行期間動態為物件添加功能
- 不想用繼承（避免類別數量爆炸）
- 功能需要可以**任意組合、疊加**
- 例：I/O Stream 包裝、UI 元件加邊框/捲軸、咖啡廳配料系統、權限中介層

**不適用的情況：**
- 物件架構需要依賴具體類別型別（Decorator 隱藏了原本的型別）
- 功能組合固定且少，直接用繼承反而更清楚

---

## 🔑 一句話記憶

> **「像洋蔥一樣層層包裹，每層都能加料」**  
> Decorator 讓功能擴充像堆積木一樣，每個裝飾者只負責自己的一小塊，  
> 透過介面串接，可以無限疊加且彼此獨立。
