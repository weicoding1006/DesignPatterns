# 多型 (Polymorphism)

多型（Polymorphism）是物件導向程式設計（OOP）的四大核心原則之一。它的意思是「**同一個介面，可以有多種不同的行為**」。透過多型，我們可以用統一的方式呼叫方法，讓各個子類別自行決定如何回應。

## 核心概念

1.  **同一介面，不同實作**：父類別定義一個通用的方法，各子類別可以用 `override` 覆寫它，提供自己專屬的行為。
2.  **`virtual` 關鍵字**：父類別在方法前加上 `virtual`，表示「這個方法允許子類別覆寫」。
3.  **`override` 關鍵字**：子類別在方法前加上 `override`，表示「我要取代父類別的這個方法」。

---

## 範例解析：交通工具系統

我們透過 `Vehicle`、`Plane` 與 `Motorcycle` 來觀察多型的運作：

### 1. 父類別：`Vehicle.cs`

`Vehicle` 使用 `virtual` 定義了可被覆寫的 `Start()` 和 `Stop()` 方法，作為所有交通工具的「預設行為」。

```csharp
public class Vehicle
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }

    public Vehicle(string brand, string model, int year)
    {
        Brand = brand;
        Model = model;
        Year = year;
    }

    // virtual 表示子類別可以覆寫這個方法
    public virtual void Start()
    {
        Console.WriteLine($"{Brand} {Model} is starting.");
    }

    public virtual void Stop()
    {
        Console.WriteLine($"{Brand} {Model} is stopping.");
    }
}
```

### 2. 子類別：`Plane.cs`

`Plane` 繼承自 `Vehicle`，並用 `override` **覆寫**了 `Start()` 和 `Stop()`，提供專屬於飛機的行為。

```csharp
public class Plane : Vehicle
{
    public Plane(string brand, string model, int year) : base(brand, model, year) {}

    // override 覆寫父類別的方法，定義飛機專屬的行為
    public override void Start()
    {
        Console.WriteLine("飛機正在飛");
    }

    public override void Stop()
    {
        Console.WriteLine("飛機停止");
    }
}
```

### 3. 子類別：`Motorcycle.cs`

`Motorcycle` 同樣繼承自 `Vehicle`，但**沒有覆寫**任何方法。因此呼叫時，它會直接使用父類別 `Vehicle` 的預設行為。

```csharp
public class Motorcycle : Vehicle
{
    public Motorcycle(string brand, string model, int year) : base(brand, model, year) {}
}
```

---

## 多型的威力：統一呼叫

多型最強大的地方，在於可以用**父類別型別**來統一管理不同的子物件，並在呼叫時自動執行對應子類別的方法：

```csharp
List<Vehicle> vehicles = new List<Vehicle>
{
    new Plane("Boeing", "747", 2020),
    new Motorcycle("Harley-Davidson", "Sportster", 2022),
};

// 用同樣的方式呼叫 Start()，但各自執行自己的行為
foreach (Vehicle v in vehicles)
{
    v.Start();
}

// 輸出：
// 飛機正在飛
// Harley-Davidson Sportster is starting.
```

> [!TIP]
> 注意：`Plane` 呼叫了自己覆寫的版本，而 `Motorcycle` 沒有覆寫，所以使用了父類別 `Vehicle` 的預設版本。這就是多型的核心體現！

---

## 關鍵字總整理

| 關鍵字 | 使用者 | 用途 |
|--------|--------|------|
| `virtual` | 父類別 | 宣告此方法「可以被子類別覆寫」 |
| `override` | 子類別 | 覆寫父類別的 `virtual` 方法，提供專屬行為 |

## 多型的好處

*   **彈性擴充**：想新增一種交通工具（例如 `Ship`），只需建立新類別並覆寫方法，不需修改任何現有程式碼。
*   **統一管理**：可以用 `List<Vehicle>` 統一存放所有交通工具，以相同方式呼叫，無需針對每個型別寫不同的邏輯。
*   **符合開放封閉原則**：系統對「擴充」開放，對「修改」封閉。
