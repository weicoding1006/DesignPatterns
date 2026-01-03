# 繼承 (Inheritance)

繼承（Inheritance）是物件導向程式設計（OOP）的四大核心原則之一。它允許一個類別（子類別）繼承另一個類別（父類別）的屬性和行為，從而實現**程式碼重用**並建立類別之間的**層級關係**。

## 核心概念

1.  **父類別 (Base Class / Parent Class)**：定義通用屬性和行為的類別。
2.  **子類別 (Derived Class / Child Class)**：繼承父類別並可以擴充或修改其行為的類別。
3.  **IS-A 關係**：子類別「是一種」父類別。例如：汽車「是一種」交通工具。

---

## 範例解析：交通工具系統

我們透過 `Vehicle`、`Car` 與 `Bike` 來觀察繼承的運作：

### 1. 父類別：`Vehicle.cs`
定義了所有交通工具共有的特徵（品牌、型號、年份）和行為（啟動、停止）。

```csharp
public class Vehicle
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }

    public Vehicle(string brand, string model, int year)
    {
        Brand = brand;
        Model = model;
        Year = year;
    }

    public void Start() => Console.WriteLine($"{Brand} {Model} is starting.");
    public void Stop() => Console.WriteLine($"{Brand} {Model} is stopping.");
}
```

### 2. 子類別：`Car.cs`
`Car` 繼承自 `Vehicle`（使用 `:` 符號），並增加了專屬於汽車的屬性 `NumberOfDoors`。

```csharp
public class Car : Vehicle
{
    public int NumberOfDoors { get; set; }

    // 使用 base 關鍵字呼叫父類別的構造函數
    public Car(string brand, string model, int year, int numberOfDoors) 
        : base(brand, model, year)
    {
        NumberOfDoors = numberOfDoors;
    }
}
```

### 3. 子類別：`Bike.cs`
`Bike` 同樣繼承自 `Vehicle`，它重用了父類別的所有邏輯，且目前沒有額外的擴充。

```csharp
public class Bike : Vehicle
{
    public Bike(string brand, string model, int year) 
        : base(brand, model, year)
    {
    }
}
```

---

## 繼承的關鍵點

*   **程式碼重用**：`Car` 和 `Bike` 不需要重新寫 `Start()` 或 `Stop()` 方法，它們自動擁有這些功能。
*   **`base` 關鍵字**：用於在子類別中存取父類別的成員或構造函數。
*   **擴充性**：子類別可以添加自己特有的屬性（如 `Car` 的車門數）或方法。

## 繼承的好處

*   **減少重複**：通用的邏輯只需要寫一次。
*   **易於維護**：如果需要修改「啟動」的邏輯，只需在 `Vehicle` 中改動一次，所有子類別都會受益。
*   **結構清晰**：定義了清晰的物件階層。
