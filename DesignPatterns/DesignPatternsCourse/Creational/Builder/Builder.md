# Builder 模式（建造者模式）

## 1. 什麼是 Builder 模式？

Builder 屬於**創建型設計模式 (Creational Design Pattern)**，核心思想是：

> **「將複雜物件的建構過程（步驟）與其最終表示方式（成品）分離，讓同樣的建構過程可以產生不同的成品。」**

這裡的關鍵詞是**「步驟與成品分離」**。Builder 不是建立一族相關物件（那是 Abstract Factory 做的事），而是專注在**如何把一個複雜物件的組裝過程拆解成一連串有序的步驟**，讓這些步驟可以各自被替換或省略。

**沒有 Builder 會怎樣？**

假設要建立一個「電腦報價單」物件，初學者最直覺的寫法常見這兩種反模式：

```csharp
// 反模式 1：巨型建構子（Telescoping Constructor）
// 只要參數一多，呼叫方根本看不懂每個 null 是什麼意思
var quote = new ComputerQuote("i9-14900K", "RTX 4090", "64GB DDR5",
                               "2TB NVMe", "850W 80+ Gold", null, null, true);

// 反模式 2：到處 new + setter
var quote = new ComputerQuote();
quote.CPU = "i9-14900K";
quote.GPU = "RTX 4090";
// 忘了設必要欄位？執行時才爆炸
```

第一種讓參數意義不明；第二種可能在物件「建到一半」時就被誤用，且強制物件必須公開所有 setter（破壞封裝）。

---

## 2. 本範例的設計：電腦報價單系統

以一個電腦報價單系統作為範例：
業務人員需要依照客戶需求，組裝「辦公文書機」、「電競旗艦機」兩種不同規格的電腦報價單。
兩種報價單的零件種類相同（CPU、記憶體、儲存空間、顯卡…），但規格與組合邏輯完全不同。

### 架構圖

```
┌──────────────────────────────────────────────────────────────┐
│                 <<interface>> IComputerBuilder               │
│  + SetCPU(string)                                            │
│  + SetGPU(string)                                            │
│  + SetRAM(string)                                            │
│  + SetStorage(string)                                        │
│  + SetPSU(string)                                            │
│  + GetResult() : ComputerQuote                               │
└──────────────────────┬───────────────────────────────────────┘
                       │ 實作
        ┌──────────────┴────────────────┐
        │                              │
┌───────┴──────────────┐   ┌───────────┴──────────────┐
│  OfficeComputerBuilder│   │  GamingComputerBuilder    │
│  (具體建造者 A)        │   │  (具體建造者 B)            │
│  SetCPU(...)         │   │  SetCPU(...)              │
│  GetResult()         │   │  GetResult()              │
└──────────────────────┘   └───────────────────────────┘
        │ 建造                         │ 建造
        └──────────────┬───────────────┘
                       │
              ┌────────┴────────┐
              │  ComputerQuote  │    ← Product（成品）
              │  CPU            │
              │  GPU            │
              │  RAM            │
              │  Storage        │
              │  PSU            │
              │  Show()         │
              └─────────────────┘

┌──────────────────────────────────┐
│  QuoteDirector（指揮者）           │
│  - _builder : IComputerBuilder   │
│  + BuildOfficePC()               │    控制步驟順序與組合
│  + BuildGamingPC()               │
└──────────────────────────────────┘
```

---

## 3. 程式碼

### 3.1 Product：`ComputerQuote`（成品 — 電腦報價單）

```csharp
// 最終被建造出的複雜物件
// 注意：所有屬性都有預設值或可為 null，建造者依需求填入
public class ComputerQuote
{
    public string CPU     { get; set; } = "（未配置）";
    public string GPU     { get; set; } = "（內顯）";
    public string RAM     { get; set; } = "（未配置）";
    public string Storage { get; set; } = "（未配置）";
    public string PSU     { get; set; } = "（未配置）";

    public void Show(string title)
    {
        Console.WriteLine($"  ┌─── {title} ───────────────────");
        Console.WriteLine($"  │  CPU     : {CPU}");
        Console.WriteLine($"  │  GPU     : {GPU}");
        Console.WriteLine($"  │  RAM     : {RAM}");
        Console.WriteLine($"  │  Storage : {Storage}");
        Console.WriteLine($"  │  PSU     : {PSU}");
        Console.WriteLine($"  └──────────────────────────────────");
    }
}
```

---

### 3.2 Builder Interface：`IComputerBuilder`（建造者介面）

```csharp
// 宣告所有組裝步驟的介面
// 每個方法回傳 IComputerBuilder，支援 Fluent 鏈式呼叫（可選）
public interface IComputerBuilder
{
    IComputerBuilder SetCPU(string cpu);
    IComputerBuilder SetGPU(string gpu);
    IComputerBuilder SetRAM(string ram);
    IComputerBuilder SetStorage(string storage);
    IComputerBuilder SetPSU(string psu);
    ComputerQuote GetResult();
}
```

> **為什麼方法回傳 `IComputerBuilder`？**
> 這讓呼叫方可以進行 Fluent 鏈式呼叫：`builder.SetCPU("...").SetRAM("...").GetResult()`，在沒有 Director 的情境下特別方便。

---

### 3.3 ConcreteBuilder A：`OfficeComputerBuilder`（辦公文書機建造者）

```csharp
// 具體建造者 A：依辦公文書機的需求實作各步驟
public class OfficeComputerBuilder : IComputerBuilder
{
    // 每次 GetResult() 前先 Reset，確保不同次建造互不污染
    private ComputerQuote _quote = new ComputerQuote();

    public IComputerBuilder SetCPU(string cpu)
    {
        _quote.CPU = cpu;
        return this;
    }

    public IComputerBuilder SetGPU(string gpu)
    {
        // 辦公機通常不需要獨立顯卡，忽略此步驟（或保留內顯預設值）
        Console.WriteLine("  [OfficeBuilder] 辦公機不需要獨立顯卡，跳過 GPU 步驟");
        return this;
    }

    public IComputerBuilder SetRAM(string ram)
    {
        _quote.RAM = ram;
        return this;
    }

    public IComputerBuilder SetStorage(string storage)
    {
        _quote.Storage = storage;
        return this;
    }

    public IComputerBuilder SetPSU(string psu)
    {
        _quote.PSU = psu;
        return this;
    }

    public ComputerQuote GetResult()
    {
        // 取出成品後重置，準備下一次建造
        var result = _quote;
        _quote = new ComputerQuote();
        return result;
    }
}
```

---

### 3.4 ConcreteBuilder B：`GamingComputerBuilder`（電競旗艦機建造者）

```csharp
// 具體建造者 B：注重高效能，每個步驟都配置旗艦規格
public class GamingComputerBuilder : IComputerBuilder
{
    private ComputerQuote _quote = new ComputerQuote();

    public IComputerBuilder SetCPU(string cpu)
    {
        // 電競機強制超頻版本，在規格字串前加上標記
        _quote.CPU = $"[超頻旗艦] {cpu}";
        return this;
    }

    public IComputerBuilder SetGPU(string gpu)
    {
        _quote.GPU = gpu;
        return this;
    }

    public IComputerBuilder SetRAM(string ram)
    {
        // 電競機強制雙通道，字串標注
        _quote.RAM = $"{ram} (雙通道)";
        return this;
    }

    public IComputerBuilder SetStorage(string storage)
    {
        _quote.Storage = storage;
        return this;
    }

    public IComputerBuilder SetPSU(string psu)
    {
        _quote.PSU = psu;
        return this;
    }

    public ComputerQuote GetResult()
    {
        var result = _quote;
        _quote = new ComputerQuote();
        return result;
    }
}
```

---

### 3.5 Director：`QuoteDirector`（指揮者）

```csharp
// Director 封裝「建構步驟的順序與組合」
// 呼叫方不需要知道步驟細節，只需要說「我要辦公機」或「我要電競機」
public class QuoteDirector
{
    private IComputerBuilder _builder;

    public QuoteDirector(IComputerBuilder builder)
    {
        _builder = builder;
    }

    // 允許在執行期切換建造者（例如同一個 Director 先建辦公機，再切換建電競機）
    public void SetBuilder(IComputerBuilder builder)
    {
        _builder = builder;
    }

    // 標準辦公機步驟：CPU → RAM → Storage → PSU（跳過 GPU）
    public ComputerQuote BuildOfficePC()
    {
        Console.WriteLine("  >> Director 執行辦公機建造流程");
        return _builder
            .SetCPU("Intel Core i5-14400")
            .SetGPU("（內顯，此步驟被跳過）")   // OfficeBuilder 會忽略此步驟
            .SetRAM("16GB DDR5")
            .SetStorage("512GB NVMe SSD")
            .SetPSU("500W 80+ Bronze")
            .GetResult();
    }

    // 旗艦電競機步驟：所有零件都用最高規格
    public ComputerQuote BuildGamingPC()
    {
        Console.WriteLine("  >> Director 執行電競機建造流程");
        return _builder
            .SetCPU("Intel Core i9-14900K")
            .SetGPU("NVIDIA RTX 4090 24GB")
            .SetRAM("64GB DDR5")
            .SetStorage("2TB PCIe Gen5 NVMe SSD")
            .SetPSU("1000W 80+ Platinum")
            .GetResult();
    }
}
```

**關鍵洞察：** Director 的兩個方法呼叫的是**完全相同的步驟序列**，但因為注入的 `IComputerBuilder` 不同，產出的成品就不同。Director 只管「做什麼步驟、按什麼順序」，具體「怎麼做」是 Builder 的事。

---

## 4. 參與的角色

| 角色 | 本範例對應 | 職責 |
|:---|:---|:---|
| **Builder（介面）** | `IComputerBuilder` | 宣告所有建構步驟的抽象方法 |
| **ConcreteBuilder** | `OfficeComputerBuilder`<br>`GamingComputerBuilder` | 實作具體步驟邏輯，並提供 `GetResult()` 取得成品 |
| **Director** | `QuoteDirector` | 封裝步驟的呼叫順序，讓呼叫方不感知細節 |
| **Product（成品）** | `ComputerQuote` | 最終被建造出的複雜物件 |
| **Client（呼叫方）** | `Program.cs` | 選擇 Builder 並傳給 Director，或直接操作 Builder |

---

## 5. 如何在 Program.cs 測試執行

```csharp
Console.WriteLine("=== Builder Pattern ===");

// ── 用法 1：透過 Director 建造（推薦：封裝完整流程）──
Console.WriteLine("\n[ 用法 1：透過 Director 控制建造流程 ]");

// 情境 A：建造辦公文書機
Console.WriteLine("\n--- 辦公文書機報價單 ---");
IComputerBuilder officeBuilder = new OfficeComputerBuilder();
var director = new QuoteDirector(officeBuilder);
ComputerQuote officePC = director.BuildOfficePC();
officePC.Show("辦公文書機");

// 情境 B：切換為電競旗艦機（只換 Builder，Director 不用改）
Console.WriteLine("\n--- 電競旗艦機報價單 ---");
IComputerBuilder gamingBuilder = new GamingComputerBuilder();
director.SetBuilder(gamingBuilder);
ComputerQuote gamingPC = director.BuildGamingPC();
gamingPC.Show("電競旗艦機");

// ── 用法 2：不使用 Director，呼叫方自行 Fluent 鏈式建造 ──
Console.WriteLine("\n[ 用法 2：Fluent Builder — 自訂規格，不依賴 Director ]");
var customPC = new GamingComputerBuilder()
    .SetCPU("AMD Ryzen 9 7950X")
    .SetGPU("AMD Radeon RX 7900 XTX")
    .SetRAM("128GB DDR5")
    .SetStorage("4TB PCIe Gen5 NVMe SSD")
    .SetPSU("1200W 80+ Titanium")
    .GetResult();
customPC.Show("自訂旗艦機");
```

**預期輸出：**
```
=== Builder Pattern ===

[ 用法 1：透過 Director 控制建造流程 ]

--- 辦公文書機報價單 ---
  >> Director 執行辦公機建造流程
  [OfficeBuilder] 辦公機不需要獨立顯卡，跳過 GPU 步驟
  ┌─── 辦公文書機 ───────────────────
  │  CPU     : Intel Core i5-14400
  │  GPU     : （內顯）
  │  RAM     : 16GB DDR5
  │  Storage : 512GB NVMe SSD
  │  PSU     : 500W 80+ Bronze
  └──────────────────────────────────

--- 電競旗艦機報價單 ---
  >> Director 執行電競機建造流程
  ┌─── 電競旗艦機 ───────────────────
  │  CPU     : [超頻旗艦] Intel Core i9-14900K
  │  GPU     : NVIDIA RTX 4090 24GB
  │  RAM     : 64GB DDR5 (雙通道)
  │  Storage : 2TB PCIe Gen5 NVMe SSD
  │  PSU     : 1000W 80+ Platinum
  └──────────────────────────────────

[ 用法 2：Fluent Builder — 自訂規格，不依賴 Director ]
  ┌─── 自訂旗艦機 ───────────────────
  │  CPU     : [超頻旗艦] AMD Ryzen 9 7950X
  │  GPU     : AMD Radeon RX 7900 XTX
  │  RAM     : 128GB DDR5 (雙通道)
  │  Storage : 4TB PCIe Gen5 NVMe SSD
  │  PSU     : 1200W 80+ Titanium
  └──────────────────────────────────
```

---

## 6. Builder 的兩種使用形式

### 6.1 GoF 完整形式（含 Director）

```
Client → Director → Builder → Product
```

- Director 封裝標準流程，呼叫方只需說「我要辦公機」
- 適合：公司內有固定且「有名字」的幾種標準組合

### 6.2 Fluent Builder（無 Director）

```
Client → Builder → Product
```

- 呼叫方自行組合步驟，靈活性更高
- 適合：每個客戶的需求都是客製的，沒有固定標準組合

> **C# 生態中最經典的 Fluent Builder：**
> ```csharp
> // ASP.NET Core 啟動流程
> var builder = WebApplication.CreateBuilder(args);
> builder.Services.AddControllers();
> builder.Services.AddDbContext<AppDbContext>(...);
> var app = builder.Build();   // ← GetResult()
>
> // EF Core 模型設定
> modelBuilder.Entity<Order>()
>     .HasKey(o => o.Id)
>     .HasIndex(o => o.CustomerId);
> ```
> 這裡的 `WebApplicationBuilder` 和 `ModelBuilder` 都是 Fluent Builder 的實際應用。

---

## 7. Builder vs 其他創建型模式

| | Builder | Abstract Factory | Factory Method |
|:---|:---|:---|:---|
| **建立目標** | **單一複雜物件**（多步驟組裝） | 多個相關物件（一個家族） | 單一物件（延遲決定類別） |
| **核心問題** | 複雜的建構步驟 / 大量可選參數 | 確保同一家族的物件一致性 | 讓子類別決定建立哪種產品 |
| **步驟概念** | ✅ 有步驟、有順序 | ❌ 一次建立（各產品獨立） | ❌ 一步建立 |
| **Director** | ✅（可選，GoF 標準形式） | ❌ | ❌ |
| **典型應用** | `WebApplicationBuilder`<br>SQL Query Builder | UI 元件家族<br>多資料庫切換 | `IHttpClientFactory`<br>框架擴展點 |

---

## 8. 在 ASP.NET Core Web API 的應用

| 場景 | Builder 概念的對應 |
|:---|:---|
| **`WebApplication.CreateBuilder(args).Build()`** | 最經典的 Fluent Builder：一步步設定服務、中介軟體，最後 `.Build()` 取得成品 |
| **EF Core `ModelBuilder`** | `modelBuilder.Entity<T>().HasKey(...).HasIndex(...)` 鏈式設定資料庫模型 |
| **`IServiceCollection` 擴展** | `builder.Services.AddXxx()` 系列：逐步組裝 DI 容器 |
| **`ContainerBuilder`（Autofac）** | Autofac 的 DI 容器採用 Builder 模式組裝 |
| **`StringBuilder`** | 最基礎的例子：逐步 Append 字元，最後 `ToString()` 取得字串成品 |
| **SQL Query Builder** | Dapper / EF Core Fluent API 逐步組合 WHERE、JOIN、ORDER BY |

---

## 9. 優缺點

### ✅ 優點

- **消滅巨型建構子**：`new Computer("i9", "RTX4090", null, null, true, false, ...)` → 換成語意清晰的步驟方法。
- **可選步驟彈性**：不相關的步驟可以跳過（如辦公機跳過 GPU），不需要傳入 `null`。
- **單一責任原則（SRP）**：產品的組裝邏輯集中在 Builder，Product 本身不負責自己被如何組裝。
- **開閉原則（OCP）**：新增一種「超值機型」→ 新增一個 `BudgetComputerBuilder`，不修改現有程式碼。
- **可重用的建造步驟**：Director 內的同一份步驟可以配合不同 Builder 產出不同成品。

### ❌ 缺點

- **類別數量增加**：每種成品需要 1 個 Builder 子類別，若成品種類很多，類別數量會增長。
- **若物件不複雜則過度設計**：物件只有 2～3 個欄位時，直接用建構子或物件初始化語法即可，不需要 Builder。

---

## 10. 何時選用 Builder？

```
你的物件是否有 >> 大量可選參數？
│
├─ YES → 是否需要固定的「標準組合」流程？
│         ├─ YES → 加上 Director（GoF 完整形式）
│         └─ NO  → Fluent Builder（呼叫方自行組合）
│
└─ NO  → 物件是否是「一族相關物件」？
          ├─ YES → Abstract Factory
          └─ NO  → 簡單的 Factory Method 或直接 new 即可
```

---

## 11. 小結

```
Builder 解決的核心問題：
┌───────────────────────────────────────────────────────────┐
│  「我的物件有大量步驟或可選零件，我不想靠一個巨型建構子   │
│   把所有東西塞進去，更不想讓物件在『建到一半』時被誤用」  │
│                                                           │
│  Builder  → 實作每個步驟的「怎麼做」                       │
│  Director → 決定步驟的「做什麼」與「先後順序」             │
│  Product  → 不關心自己是怎麼被組裝的                       │
│  Client   → 只選 Builder，其他都不用管                     │
└───────────────────────────────────────────────────────────┘
```

| | 選擇的時機 |
|---|---|
| **Builder（含 Director）** | 物件建構步驟複雜，且有幾種「有名字的標準組合」 |
| **Fluent Builder（無 Director）** | 客製化程度高，呼叫方自行組合步驟 |
| **Abstract Factory** | 需要一次建立「一族相關物件」，確保風格一致 |
