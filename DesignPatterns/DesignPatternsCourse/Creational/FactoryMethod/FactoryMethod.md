# Factory Method 模式（工廠方法模式）

## 1. 什麼是 Factory Method 模式？

Factory Method 屬於**創建型設計模式 (Creational Design Pattern)**，核心思想是：

> **「在父類別中定義建立物件的介面（工廠方法），但將『要建立哪種具體物件』的決定權，交給子類別。」**

這裡的重點是：父類別只知道「需要某種產品」，但**不知道、也不決定**是哪一種具體產品。子類別才是真正做決定的那一方。

**沒有 Factory Method 會怎樣？**

假設你的配送系統一開始只有卡車：
```csharp
// 直接在業務邏輯中 new 具體類別 → 緊密耦合
public void PlanDelivery(string cargo)
{
    var transport = new Truck(); // 寫死了！
    transport.Deliver(cargo);
}
```
日後業務擴展到海外，需要改用輪船，你必須**直接修改這段業務邏輯的程式碼**，違反了「開閉原則（OCP）」——對修改封閉，對擴展開放。

---

## 2. 本範例的設計：物流配送系統

以一個物流公司的配送系統作為範例，說明如何透過 Factory Method 讓「業務邏輯」與「實際使用哪種交通工具」完全解耦。

### 架構圖

```
         ┌─────────────────────────────────────────┐
         │         LogisticsCreator (抽象創建者)     │
         │  + CreateTransport() : ITransport  ← 工廠方法 (abstract)
         │  + PlanDelivery(cargo)             ← 高層業務邏輯 (使用工廠方法的結果)
         └──────────────────┬──────────────────────┘
                            │ 繼承
          ┌─────────────────┴─────────────────┐
          │                                   │
  ┌───────┴────────┐                 ┌────────┴───────┐
  │  RoadLogistics │                 │  SeaLogistics  │
  │ (具體創建者 A)  │                 │  (具體創建者 B) │
  │ 覆寫工廠方法   │                 │  覆寫工廠方法   │
  │ return Truck() │                 │  return Ship() │
  └───────┬────────┘                 └────────┬───────┘
          │ 建立                              │ 建立
          ▼                                   ▼
     ┌─────────┐                        ┌─────────┐
     │  Truck  │                        │  Ship   │
     │(具體產品)│                        │(具體產品)│
     └─────────┘                        └─────────┘
          └──────────────┬──────────────┘
                         │ 實作
                  ┌──────┴──────┐
                  │ ITransport  │
                  │ (產品介面)   │
                  │ Deliver()   │
                  └─────────────┘
```

---

## 3. 程式碼

### 3.1 Product 介面：`ITransport`

```csharp
public interface ITransport
{
    void Deliver(string cargo);
}
```

定義所有運輸工具共同的行為契約。呼叫方只依賴此介面，完全不感知是卡車或輪船。

---

### 3.2 ConcreteProduct：`Truck` 與 `Ship`

```csharp
// 卡車：走陸路
public class Truck : ITransport
{
    public void Deliver(string cargo)
    {
        Console.WriteLine($"[卡車🚛] 透過公路運送：{cargo}");
    }
}

// 輪船：走海路
public class Ship : ITransport
{
    public void Deliver(string cargo)
    {
        Console.WriteLine($"[輪船🚢] 透過海路運送：{cargo}");
    }
}
```

---

### 3.3 Creator（抽象創建者）：`LogisticsCreator`

```csharp
public abstract class LogisticsCreator
{
    // ★ 工廠方法：由子類別決定要建立哪種產品
    public abstract ITransport CreateTransport();

    // 高層業務邏輯：只依賴 ITransport 介面，不感知具體類別
    public void PlanDelivery(string cargo)
    {
        Console.WriteLine("--- 開始規劃配送 ---");
        ITransport transport = CreateTransport(); // 呼叫工廠方法
        transport.Deliver(cargo);
        Console.WriteLine("--- 配送完成 ---\n");
    }
}
```

**關鍵洞察：** `PlanDelivery` 是「使用產品的高層邏輯」，它永遠不會改變。
唯一會因業務擴展而新增的，只有子類別（新的 Creator 與新的 Product）。

---

### 3.4 ConcreteCreator：`RoadLogistics` 與 `SeaLogistics`

```csharp
// 陸路物流：覆寫工廠方法，決定生產卡車
public class RoadLogistics : LogisticsCreator
{
    public override ITransport CreateTransport()
    {
        return new Truck();
    }
}

// 海路物流：覆寫工廠方法，決定生產輪船
public class SeaLogistics : LogisticsCreator
{
    public override ITransport CreateTransport()
    {
        return new Ship();
    }
}
```

---

## 4. 參與的角色

| 角色 | 本範例對應 | 職責 |
|:---|:---|:---|
| **Product（產品介面）** | `ITransport` | 定義所有具體產品共同的行為 |
| **ConcreteProduct（具體產品）** | `Truck`, `Ship` | 實作產品介面的具體邏輯 |
| **Creator（抽象創建者）** | `LogisticsCreator` | 宣告工廠方法，並包含使用產品的高層業務邏輯 |
| **ConcreteCreator（具體創建者）** | `RoadLogistics`, `SeaLogistics` | 覆寫工廠方法，決定建立哪種具體產品 |

---

## 5. 如何在 Program.cs 測試執行

```csharp
Console.WriteLine("=== Factory Method Pattern ===");

// 情境 1：目前只有陸路物流
// 呼叫方宣告為 LogisticsCreator（父類別/抽象），不在乎底層是卡車還是輪船
LogisticsCreator logistics = new RoadLogistics();
logistics.PlanDelivery("電腦零件 x 100箱");

// 情境 2：業務擴展到海外，切換為海路物流
// 呼叫方的程式碼（PlanDelivery）完全不用改動！
logistics = new SeaLogistics();
logistics.PlanDelivery("重型機械 x 5台");
```

**預期輸出：**
```
=== Factory Method Pattern ===
--- 開始規劃配送 ---
[卡車🚛] 透過公路運送：電腦零件 x 100箱
--- 配送完成 ---

--- 開始規劃配送 ---
[輪船🚢] 透過海路運送：重型機械 x 5台
--- 配送完成 ---
```

**驗證重點：** 從陸路切換為海路時，`PlanDelivery()` 的程式碼一行都沒有改動，我們只是換了不同的 Creator，這正是 Factory Method 的核心價值。

---

## 6. Factory Method vs 簡單工廠（Simple Factory）

這兩者很容易混淆，差異如下：

| | 簡單工廠（Simple Factory） | Factory Method（本範例） |
|:---|:---|:---|
| **是 GoF 設計模式？** | ❌ 不是，只是一種程式習慣 | ✅ 是 |
| **建立方式** | 集中在單一工廠類別，用 `if/switch` 判斷 | 透過繼承，子類別各自決定 |
| **新增產品** | 需修改工廠類別（違反 OCP） | 只需新增新的子類別（符合 OCP） |
| **業務邏輯位置** | 不在工廠內 | 在抽象 Creator 內 |

**簡單工廠的問題：**
```csharp
// 每次新增運輸工具，都得修改這個方法 → 違反開閉原則
public static ITransport Create(string type)
{
    return type switch
    {
        "road" => new Truck(),
        "sea"  => new Ship(),
        // 新增飛機？就得改這裡！
        _ => throw new ArgumentException("未知類型")
    };
}
```

---

## 7. 在 ASP.NET Core Web API 的應用

Factory Method 在框架層面隨處可見，你幾乎每天都在使用它而不自知：

| 場景 | Creator | ConcreteCreator | Product |
|:---|:---|:---|:---|
| **`IHttpClientFactory`** | `IHttpClientFactory` | 各種具名客戶端 | `HttpClient` |
| **EF Core Provider** | `DbContextOptionsBuilder` | `UseSqlServer()`, `UseSqlite()` | `DbContext` 內部連線實作 |
| **ASP.NET Core 中介軟體** | `IMiddlewareFactory` | 自訂 Middleware Factory | `IMiddleware` |
| **Controller Action Result** | `ControllerBase` | 各 Controller 子類別 | `IActionResult`（`Ok()`, `NotFound()` 等） |

> `ControllerBase.Ok()`, `NotFound()`, `BadRequest()` 這些方法，本質上就是工廠方法！
> 它們被定義在父類別 `ControllerBase`，子控制器直接呼叫，不需要自己 `new OkObjectResult()`。

---

## 8. 何時新增產品（擴展範例）

若未來業務需要「航空運輸」，你只需要：

**① 新增具體產品：**
```csharp
public class Airplane : ITransport
{
    public void Deliver(string cargo)
    {
        Console.WriteLine($"[飛機✈️] 透過航空運送：{cargo}");
    }
}
```

**② 新增具體創建者：**
```csharp
public class AirLogistics : LogisticsCreator
{
    public override ITransport CreateTransport()
    {
        return new Airplane();
    }
}
```

**③ 呼叫方：**
```csharp
LogisticsCreator logistics = new AirLogistics();
logistics.PlanDelivery("急件文件 x 1份");
```

`PlanDelivery()` 的程式碼**完全沒有動到**，`LogisticsCreator`、`ITransport` 也沒有動。完全符合**開閉原則（OCP）**。

---

## 9. 優缺點

### ✅ 優點

- **符合開閉原則（OCP）**：新增產品只需新增子類別，不修改現有程式碼。
- **單一職責原則（SRP）**：建立產品的程式碼與使用產品的業務邏輯分開，各司其職。
- **降低耦合**：呼叫方（Creator）只依賴介面（ITransport），不依賴具體類別（Truck/Ship）。

### ❌ 缺點

- **類別數量增加**：每種新產品都需要對應一個新的 ConcreteCreator，類別數量會隨產品種類成長。  
  （若產品種類只有 2-3 種且穩定不變，這個模式可能是過度設計）

---

## 10. 小結

```
Factory Method 解決的核心問題：
┌─────────────────────────────────────────────────┐
│  父類別（Creator）：「我知道需要某種交通工具」    │
│  父類別（Creator）：「但我不決定是哪一種」       │
│  子類別（ConcreteCreator）：「讓我來決定！」     │
└─────────────────────────────────────────────────┘
```

| | 決策者 |
|---|---|
| **業務流程（如何配送）** | 父類別 `LogisticsCreator.PlanDelivery()` 決定 |
| **建立哪種產品（卡車/輪船）** | 子類別 `RoadLogistics` / `SeaLogistics` 決定 |

> Factory Method 讓「框架/父類別」能定義整體流程，同時給「子類別/使用者」足夠的彈性來客製化其中的關鍵步驟（物件建立）。
