# Abstract Factory 模式（抽象工廠模式）

## 1. 什麼是 Abstract Factory 模式？

Abstract Factory 屬於**創建型設計模式 (Creational Design Pattern)**，核心思想是：

> **「提供一個介面，讓呼叫方可以建立『一個系列（家族）的相關物件』，而不需要指定它們的具體類別。」**

這裡的關鍵詞是**「一個家族」**。Abstract Factory 不只是建立一個物件，而是確保同時建立的**多種物件**都來自同一個風格家族，不會發生「Windows 按鈕」搭配「Mac 核取方塊」這種風格不一致的情況。

**沒有 Abstract Factory 會怎樣？**

如果不使用此模式，呼叫方可能需要自己判斷平台並分開建立各個元件：
```csharp
// 到處散落的 if/else，且混用風格的風險很高
IButton button;
ICheckbox checkbox;

if (platform == "Windows")
{
    button = new WindowsButton();
    checkbox = new WindowsCheckbox();
}
else
{
    button = new MacButton();
    checkbox = new MacCheckbox(); // 如果這裡忘了改？就會「Windows 按鈕 + Mac Checkbox」
}
```
一旦忘記同步修改，或日後新增 Linux 平台，就得到處找這些散落的 `if/else`。

---

## 2. 本範例的設計：跨平台 UI 元件系統

以一個跨平台桌面應用程式的 UI 元件系統作為範例：
同一套應用程式，在 Windows 與 Mac 上執行時，所有 UI 元件（按鈕、核取方塊）都必須保持平台原生的視覺風格。

### 架構圖

```
┌──────────────────────────────────────────────────────────────┐
│                    <<interface>> IUIFactory                   │
│               + CreateButton()   : IButton                   │
│               + CreateCheckbox() : ICheckbox                 │
└──────────────────────┬───────────────────────────────────────┘
                       │ 實作
        ┌──────────────┴─────────────┐
        │                            │
┌───────┴──────────┐      ┌──────────┴──────────┐
│ WindowsUIFactory │      │    MacUIFactory      │
│(具體工廠 A)       │      │   (具體工廠 B)       │
│ CreateButton()   │      │  CreateButton()      │
│ CreateCheckbox() │      │  CreateCheckbox()    │
└───────┬──────────┘      └──────────┬───────────┘
        │ 建立                        │ 建立
   ┌────┴──────┐                ┌─────┴──────┐
   │           │                │            │
   ▼           ▼                ▼            ▼
Windows    Windows           Mac          Mac
Button     Checkbox          Button       Checkbox
   │           │                │            │
   └─────┬─────┘                └─────┬──────┘
         │ 實作                        │ 實作
   ┌─────┴──────┐               ┌─────┴──────┐
   │  IButton   │               │ ICheckbox  │
   │ (抽象產品A) │               │ (抽象產品B) │
   └────────────┘               └────────────┘
```

---

## 3. 程式碼

### 3.1 Abstract Product：`IButton` 與 `ICheckbox`

```csharp
// 抽象產品 A：所有平台的按鈕共同行為
public interface IButton
{
    void Render();
    void OnClick();
}

// 抽象產品 B：所有平台的核取方塊共同行為
public interface ICheckbox
{
    void Render();
    void Toggle();
}
```

---

### 3.2 Abstract Factory：`IUIFactory`

```csharp
// 抽象工廠：宣告建立「一整個家族」的方法
public interface IUIFactory
{
    IButton CreateButton();
    ICheckbox CreateCheckbox();
}
```

每個方法對應一種抽象產品。工廠負責確保所有方法建立的產品**來自同一家族**。

---

### 3.3 ConcreteProduct：Windows 與 Mac 各自的元件

```csharp
// Windows 家族
public class WindowsButton : IButton
{
    public void Render()   => Console.WriteLine("  [Windows Button] 渲染矩形風格按鈕 [ OK ]");
    public void OnClick()  => Console.WriteLine("  [Windows Button] 點擊：播放 Windows 系統音效 *叮*");
}

public class WindowsCheckbox : ICheckbox
{
    private bool _isChecked = false;
    public void Render()   => Console.WriteLine($"  [Windows Checkbox] 方形核取方塊：{(_isChecked ? "[✓]" : "[ ]")}");
    public void Toggle()   { _isChecked = !_isChecked; Console.WriteLine($"  [Windows Checkbox] 切換為：{(_isChecked ? "已勾選 ✓" : "未勾選")}"); }
}

// Mac 家族
public class MacButton : IButton
{
    public void Render()   => Console.WriteLine("  [Mac Button] 渲染圓角風格按鈕 ( OK )");
    public void OnClick()  => Console.WriteLine("  [Mac Button] 點擊：觸覺回饋 + 淡入淡出動畫");
}

public class MacCheckbox : ICheckbox
{
    private bool _isOn = false;
    public void Render()   => Console.WriteLine($"  [Mac Checkbox] 圓形切換開關：{(_isOn ? "● ON " : "○ OFF")} （滑動動畫）");
    public void Toggle()   { _isOn = !_isOn; Console.WriteLine($"  [Mac Checkbox] 切換為：{(_isOn ? "ON ●" : "OFF ○")}"); }
}
```

---

### 3.4 ConcreteFactory：`WindowsUIFactory` 與 `MacUIFactory`

```csharp
// 具體工廠 A：生產所有 Windows 風格元件
public class WindowsUIFactory : IUIFactory
{
    public IButton CreateButton()     => new WindowsButton();
    public ICheckbox CreateCheckbox() => new WindowsCheckbox();
}

// 具體工廠 B：生產所有 Mac 風格元件
public class MacUIFactory : IUIFactory
{
    public IButton CreateButton()     => new MacButton();
    public ICheckbox CreateCheckbox() => new MacCheckbox();
}
```

**關鍵洞察：** 每個具體工廠只生產**同一風格家族**的元件，這從架構層面保證了視覺一致性，而不依賴開發者的自律。

---

### 3.5 Client：`UIApplication`

```csharp
public class UIApplication
{
    private readonly IButton _button;
    private readonly ICheckbox _checkbox;

    // 只依賴抽象工廠，具體是哪個工廠由呼叫方在外部決定（依賴注入）
    public UIApplication(IUIFactory factory)
    {
        _button   = factory.CreateButton();
        _checkbox = factory.CreateCheckbox();
    }

    public void RenderUI()
    {
        Console.WriteLine("  >> 渲染介面元件");
        _button.Render();
        _checkbox.Render();
    }

    public void SimulateInteraction()
    {
        Console.WriteLine("  >> 模擬使用者互動");
        _button.OnClick();
        _checkbox.Toggle();
        _checkbox.Render();
    }
}
```

`UIApplication` 是真正的呼叫方（Client）。它**完全不知道**自己用的是 Windows 元件還是 Mac 元件，只知道手上有 `IButton` 和 `ICheckbox`。

---

## 4. 參與的角色

| 角色 | 本範例對應 | 職責 |
|:---|:---|:---|
| **Abstract Factory** | `IUIFactory` | 宣告建立一系列相關產品的介面 |
| **Concrete Factory** | `WindowsUIFactory`, `MacUIFactory` | 實作建立具體產品家族的邏輯 |
| **Abstract Product** | `IButton`, `ICheckbox` | 定義每種產品的共同行為 |
| **Concrete Product** | `WindowsButton`, `MacButton`... 等 | 具體實作各平台的元件外觀與行為 |
| **Client** | `UIApplication` | 只依賴抽象介面，由工廠提供具體物件 |

---

## 5. 如何在 Program.cs 測試執行

```csharp
Console.WriteLine("=== Abstract Factory Pattern ===");

// 情境 1：在 Windows 上執行，注入 Windows 工廠
Console.WriteLine("\n[ 平台：Windows ]");
IUIFactory factory = new WindowsUIFactory();
var app = new UIApplication(factory);
app.RenderUI();
app.SimulateInteraction();

// 情境 2：切換到 Mac，只換工廠，UIApplication 的程式碼一行都不改
Console.WriteLine("\n[ 平台：Mac ]");
factory = new MacUIFactory();
app = new UIApplication(factory);
app.RenderUI();
app.SimulateInteraction();
```

**預期輸出：**
```
=== Abstract Factory Pattern ===

[ 平台：Windows ]
  >> 渲染介面元件
  [Windows Button] 渲染矩形風格按鈕 [ OK ]
  [Windows Checkbox] 方形核取方塊：[ ]
  >> 模擬使用者互動
  [Windows Button] 點擊：播放 Windows 系統音效 *叮*
  [Windows Checkbox] 切換為：已勾選 ✓
  [Windows Checkbox] 方形核取方塊：[✓]

[ 平台：Mac ]
  >> 渲染介面元件
  [Mac Button] 渲染圓角風格按鈕 ( OK )
  [Mac Checkbox] 圓形切換開關：○ OFF （滑動動畫）
  >> 模擬使用者互動
  [Mac Button] 點擊：觸覺回饋 + 淡入淡出動畫
  [Mac Checkbox] 切換為：ON ●
  [Mac Checkbox] 圓形切換開關：● ON  （滑動動畫）
```

**驗證重點：** 切換平台時，`UIApplication` 的程式碼**完全沒有動**，只要替換注入的工廠即可。風格一致性由工廠架構本身保證。

---

## 6. Abstract Factory vs Factory Method

這兩個模式很容易混淆，以下是核心差異：

| | Factory Method | Abstract Factory |
|:---|:---|:---|
| **建立物件數量** | 處理**單一**產品的建立 | 處理**一系列相關**產品的建立 |
| **機制** | 繼承（子類別覆寫工廠方法） | 組合（呼叫方持有工廠物件） |
| **擴展維度** | 新增產品 → 新增一個子類別 | 新增產品家族 → 新增一個具體工廠 |
| **典型應用** | 框架讓子類別決定產品類型 | 確保同一家族的物件一起被使用 |

> **關係：** Abstract Factory 內部的每個 `CreateXxx()` 方法，本質上就是一個 Factory Method！
> Abstract Factory 可以理解為：**「一組 Factory Method 的集合，它們共同生產同一風格家族的產品」**。

---

## 7. 何時新增產品家族（擴展範例）

若日後需要支援 **Linux** 平台，只需：

**① 新增具體產品：**
```csharp
public class LinuxButton : IButton
{
    public void Render()  => Console.WriteLine("  [Linux Button] 渲染 GTK 風格按鈕");
    public void OnClick() => Console.WriteLine("  [Linux Button] 點擊：D-Bus 事件發送");
}

public class LinuxCheckbox : ICheckbox { /* ... */ }
```

**② 新增具體工廠：**
```csharp
public class LinuxUIFactory : IUIFactory
{
    public IButton CreateButton()     => new LinuxButton();
    public ICheckbox CreateCheckbox() => new LinuxCheckbox();
}
```

**③ 呼叫方：**
```csharp
IUIFactory factory = new LinuxUIFactory();
var app = new UIApplication(factory);
app.RenderUI();
```

`UIApplication`、`IUIFactory`、`IButton`、`ICheckbox` 全部**完全沒有動**，完全符合**開閉原則（OCP）**。

---

## 8. 在 ASP.NET Core Web API 的應用

| 場景 | Abstract Factory 概念的對應 |
|:---|:---|
| **多資料庫切換（SQL Server / PostgreSQL）** | `IDbContextFactory` 根據環境建立對應的 `DbContext` 與 `DbConnection` |
| **多雲端儲存（Azure Blob / AWS S3）** | `IStorageFactory` 建立同一套 `IFileStorage` + `IFileStream` 實作 |
| **多環境設定（Dev / Staging / Prod）** | `IInfrastructureFactory` 建立對應的 Logger、Cache、Queue 服務 |
| **主題化 UI 系統（Dark / Light Mode）** | 工廠決定一整組元件的主題樣式 |

---

## 9. 優缺點

### ✅ 優點

- **產品家族一致性保證**：工廠架構本身從源頭確保不會出現「Windows 按鈕 + Mac Checkbox」的錯誤搭配。
- **符合開閉原則（OCP）**：新增產品家族（如 Linux）只需新增類別，不修改任何現有程式碼。
- **解耦呼叫方與具體類別**：Client（`UIApplication`）完全不感知具體實作，只依賴抽象介面。

### ❌ 缺點

- **新增產品種類困難**：若要在現有家族中加入一種新產品（如 `ISlider`），需要修改 `IUIFactory` 介面，**所有具體工廠都必須跟著新增實作**，可能影響面很廣。
- **類別數量快速增加**：$M$ 種工廠 × $N$ 種產品 = $M \times N$ 個具體類別，維護成本隨規模上升。

---

## 10. 小結

```
Abstract Factory 解決的核心問題：
┌───────────────────────────────────────────────────────────┐
│  「我需要一整組 UI 元件，但它們必須來自同一個風格家族」    │
│                                                           │
│  工廠    → 決定整個家族（Windows / Mac / Linux）          │
│  產品    → 家族內的各個元件（Button / Checkbox / ...）    │
│  Client  → 只跟介面說話，不在乎背後是哪個平台             │
└───────────────────────────────────────────────────────────┘
```

| | 選擇的時機 |
|---|---|
| **Factory Method** | 「我需要建立**一種**產品，但讓子類別決定是哪個具體類別」 |
| **Abstract Factory** | 「我需要建立**一整套相關產品**，且它們必須風格一致」 |
