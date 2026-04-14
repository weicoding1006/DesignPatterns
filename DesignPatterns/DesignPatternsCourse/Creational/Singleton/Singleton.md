# Singleton 模式（單例模式）

## 1. 什麼是 Singleton 模式？

Singleton 屬於**創建型設計模式 (Creational Design Pattern)**，核心思想是：

> **「確保一個類別在整個應用程式的生命週期中，只存在唯一一個實例，並提供一個全域的存取點。」**

簡單說，就像公司只有一位 CEO，無論哪個部門要請示，找的都是同一個人。

**沒有 Singleton 會怎樣？**
若每次需要讀取設定檔時都 `new AppConfig()`，每個呼叫方拿到的都是不同的物件，彼此的狀態互不同步。一旦有人修改版本號，其他人看到的依然是舊值，造成資料不一致的 Bug。

---

## 2. 本範例的類別：`AppConfig`

這個範例模擬一個應用程式設定物件，在整個系統中只應存在一份。

```csharp
namespace DesignPatterns.DesignPatternsCourse.Creational.Singleton
{
    public sealed class AppConfig
    {
        private static readonly AppConfig _instance = new AppConfig();
        public static AppConfig Instance => _instance;

        public string ApplicationName { get; private set; }
        public string Version { get; private set; }

        private AppConfig()
        {
            ApplicationName = "設計模式教學";
            Version = "1.0.0";
        }

        public void UpdateVersion(string version)
        {
            Version = version;
        }
    }
}
```

---

## 3. 程式碼解析

### 3.1 三個關鍵設計決策

#### ① `private static readonly AppConfig _instance = new AppConfig();`

這是 **「靜態初始化（Static Initialization）」** 的寫法。
- `static`：欄位屬於類別本身，不屬於任何實例，全程只存在一份。
- `readonly`：欄位只在靜態初始化階段被賦值，之後無法被替換，保護了實例不被外部竄改。
- 在類別第一次被系統載入時，CLR（.NET 執行環境）會**自動且只執行一次**建立動作。

> **執行緒安全（Thread-Safe）補充：**
> 靜態欄位的初始化由 CLR 保證只執行一次，即使在多執行緒環境下也不會發生兩個執行緒同時初始化的情況。這是此寫法最大的優點。

---

#### ② `public static AppConfig Instance => _instance;`

這是對外提供的**唯一存取點（Global Access Point）**。
- 呼叫方透過 `AppConfig.Instance` 取得物件，永遠拿到的都是同一個實例。
- `=>` 是屬性的 Expression Body 語法，等同於 `get { return _instance; }`。

---

#### ③ `private AppConfig()` 私有建構子

這是 Singleton 的安全鎖。
- 外部程式碼**無法**執行 `new AppConfig()`，編譯器直接報錯。
- 確保物件只能由類別自身在靜態初始化時建立，完全杜絕意外產生第二個實例的可能。

---

#### ④ `public sealed class AppConfig`

`sealed` 關鍵字**禁止其他類別繼承 `AppConfig`**。
- 若允許繼承，子類別可以覆寫行為，甚至繞過私有建構子的限制（透過子類別的建構子），破壞 Singleton 保證。
- 加上 `sealed` 是完整 Singleton 實作的最後一道防線。

---

### 3.2 角色對應

| 角色 | 本範例對應 | 說明 |
|:---|:---|:---|
| Singleton Class | `AppConfig` | 唯一實例的擁有者 |
| 靜態欄位 | `_instance` | 儲存唯一實例的地方 |
| 存取點 | `AppConfig.Instance` | 全域取得實例的入口 |
| 私有建構子 | `private AppConfig()` | 阻止外部建立新實例 |

---

## 4. 如何在 Program.cs 測試執行

```csharp
// Singleton 模式驗證
var config1 = AppConfig.Instance;
var config2 = AppConfig.Instance;

Console.WriteLine($"App : {config1.ApplicationName}");
Console.WriteLine($"Version: {config1.Version}");

// 透過 config1 更新版本
config1.UpdateVersion("1.1.0");

// config2 也會看到新版本，證明兩者是同一個物件
Console.WriteLine($"App : {config2.ApplicationName}");
Console.WriteLine($"Version: {config2.Version}");
```

**預期輸出：**
```
App : 設計模式教學
Version: 1.0.0
App : 設計模式教學
Version: 1.1.0
```

**驗證重點：**
`config1` 與 `config2` 雖然是兩個不同的變數，但它們指向的是**同一個 `AppConfig` 物件**。
因此 `config1.UpdateVersion("1.1.0")` 修改後，透過 `config2` 讀取版本號，得到的同樣是更新後的值 `1.1.0`。

---

## 5. Singleton 的三種常見實作方式

| 方式 | 說明 | 執行緒安全 | Lazy（延遲初始化） |
|:---|:---|:---:|:---:|
| **靜態欄位初始化（本範例）** | `private static readonly T _instance = new T()` | ✅ CLR 保證 | ❌ 類別載入時即初始化 |
| **`Lazy<T>` 延遲初始化** | `private static readonly Lazy<T> _instance = new(() => new T())` | ✅ `Lazy<T>` 保證 | ✅ 第一次存取才初始化 |
| **lock 雙重檢查鎖定** | 手動加鎖，較複雜（舊式寫法） | ✅ 需正確實作 | ✅ |

> **現代 C# 推薦使用靜態欄位初始化或 `Lazy<T>`**，不需要手動 `lock`，程式碼更簡潔安全。

---

## 6. 在 ASP.NET Core Web API 的應用

其實你每天寫 `services.AddSingleton<T>()` 時，框架背後做的事就是 Singleton 模式。

| 場景 | 說明 |
|:---|:---|
| `services.AddSingleton<IAppConfig, AppConfig>()` | 整個應用生命週期只建立一個 `AppConfig` 實例 |
| `ILogger` / `ILoggerFactory` | 全域共用的 Logger，不需要每次請求都新建 |
| `HttpClient`（透過 `IHttpClientFactory`） | 避免 Socket 耗盡，共用連線池 |
| `DbContext` 搭配 `AddSingleton`（需小心） | 若使用 Singleton，需確保 Thread-Safe |

> ⚠️ **注意：** 在 ASP.NET Core 中，`DbContext` 通常搭配 `AddScoped`（每個 HTTP Request 一個實例）而非 `AddSingleton`，因為 `DbContext` 本身**不是 Thread-Safe** 的。

---

## 7. 優缺點

### ✅ 優點

- **節省資源**：對於建立成本高的物件（如連線池、設定讀取），確保只建立一次。
- **一致性**：整個應用程式共享同一份狀態，避免多份不同步的資料副本。
- **全域存取**：提供方便的存取點，不需要到處傳遞物件參考。

### ❌ 缺點

- **單元測試困難**：靜態存取點難以被 Mock 替換，造成測試隔離困難。
  - **改善方式**：將 Singleton 背後的 `IAppConfig` 介面透過 DI 注入，測試時可輕鬆替換。
- **隱藏依賴**：`AppConfig.Instance` 是直接呼叫，在方法簽章上看不出這個依賴，不如建構子注入直觀。
- **多執行緒風險**：若實作不當（如手動 `lock` 寫錯），可能在高並發下建立多個實例。

---

## 8. 小結

```
Singleton 的三個保證：
┌─────────────────────────────────────┐
│  1. private 建構子  → 不能 new      │
│  2. static 欄位     → 全程唯一一份  │
│  3. sealed 類別     → 不能繼承      │
└─────────────────────────────────────┘
```

Singleton 是五種創建型模式中**最簡單也最常被誤用**的一種。
使用前請先問自己：**這個物件真的需要全域唯一嗎？** 若答案是肯定的（且需共享狀態），Singleton 才是正確選擇。
