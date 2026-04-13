# Facade 外觀模式

> **分類**：結構型模式（Structural Pattern）  
> **別名**：門面模式

---

## 🎯 核心概念

**Facade** 的目標是：**為複雜的子系統提供一個簡單、統一的介面**。

當系統由多個子類別組成，每個子類別都有各自的操作步驟，Client 若要直接操作會相當繁瑣且容易出錯。Facade 將這些複雜的呼叫流程封裝在一個類別內，讓 Client 只需要面對一個簡單的入口。

> 就像飯店的「服務台」：你不需要分別聯絡廚房、客房、停車場，  
> 只要找服務台，他會幫你協調好所有事情。

---

## 🗺 UML 類別圖

```
           ┌────────────────────────────┐
 Client    │     HomeTheaterFacade      │
───────────►│────────────────────────────│
           │ - _projector: Projector    │
           │ - _sound: SoundSystem      │
           │ - _lights: Lights          │
           │────────────────────────────│
           │ + WatchMovie()             │
           │ + EndMovie()               │
           └──────┬─────────┬──────────┘
                  │         │          │
            ┌─────▼──┐ ┌───▼───────┐ ┌▼──────┐
            │Projector│ │SoundSystem│ │Lights │
            │────────│ │──────────│ │───────│
            │On()    │ │On()      │ │Dim()  │
            │SetInput│ │SetVolume │ │On()   │
            │Off()   │ │Off()     │ └───────┘
            └────────┘ └──────────┘
```

---

## 📂 程式碼解析

### 子系統（Subsystems）— 各自獨立的複雜元件

#### `Projector.cs` — 投影機

```csharp
public class Projector
{
    public void On()                        => Console.WriteLine("投影機:開機");
    public void SetInput(string input)      => Console.WriteLine($"投影機:切換輸入源至:{input}");
    public void Off()                       => Console.WriteLine("投影機:關機");
}
```

#### `SoundSystem.cs` — 音響系統

```csharp
public class SoundSystem
{
    public void On()                        => Console.Write("音響:開機");
    public void SetVolume(int level)        => Console.WriteLine($"音響:音量設定為{level}");
    public void Off()                       => Console.WriteLine("音響:關機");
}
```

#### `Lights.cs` — 燈光控制

```csharp
public class Lights
{
    public void Dim(int level)              => Console.Write($"燈光:調整至{level}%");
    public void On()                        => Console.WriteLine("燈光:全亮");
}
```

---

### Facade — `HomeTheaterFacade.cs`

```csharp
public class HomeTheaterFacade
{
    private readonly Projector   _projector;
    private readonly SoundSystem _sound;
    private readonly Lights      _lights;

    public HomeTheaterFacade(Projector projector, SoundSystem sound, Lights lights)
    {
        _projector = projector;
        _sound     = sound;
        _lights    = lights;
    }

    // ✅ 封裝「看電影」的所有複雜步驟
    public void WatchMovie()
    {
        Console.WriteLine("\n --準備看電影 --");
        _lights.Dim(10);          // 1. 燈光調暗至 10%
        _projector.On();          // 2. 投影機開機
        _projector.SetInput("HDMI"); // 3. 切換輸入源
        _sound.On();              // 4. 音響開機
        _sound.SetVolume(50);     // 5. 音量設 50
    }

    // ✅ 封裝「結束電影」的所有複雜步驟
    public void EndMovie()
    {
        Console.WriteLine("\n --電影結束，關閉系統--");
        _projector.Off();         // 1. 投影機關機
        _sound.Off();             // 2. 音響關機
        _lights.On();             // 3. 燈光全亮
    }
}
```

---

### Client — `Program.cs`

```csharp
// 建立子系統元件
var projector = new Projector();
var sound     = new SoundSystem();
var lights    = new Lights();

// 透過 Facade 統一操作，不需知道子系統細節
var homeTheaterFacade = new HomeTheaterFacade(projector, sound, lights);
homeTheaterFacade.WatchMovie();
homeTheaterFacade.EndMovie();
```

#### 執行結果

```
 --準備看電影 --
燈光:調整至10%
投影機:開機
投影機:切換輸入源至:HDMI
音響:開機
音響:音量設定為50

 --電影結束，關閉系統--
投影機:關機
音響:關機
燈光:全亮
```

---

## 💡 無 Facade vs. 有 Facade 的對比

### ❌ 沒有 Facade（Client 直接操作，步驟繁瑣）

```csharp
lights.Dim(10);
projector.On();
projector.SetInput("HDMI");
sound.On();
sound.SetVolume(50);
// 還要記住順序、參數...
```

### ✅ 有 Facade（一行搞定）

```csharp
homeTheaterFacade.WatchMovie();
```

---

## ⚖️ 與其他模式比較

| 模式 | 目的 | 差異 |
|---|---|---|
| **Facade** | 簡化複雜子系統的介面 | 提供高層簡化介面，子系統仍可直接使用 |
| **Adapter** | 讓不相容介面可以合作 | 轉換介面，不簡化 |
| **Decorator** | 動態擴充物件功能 | 增加行為，不隱藏複雜度 |
| **Mediator** | 物件間解耦通訊 | 管理物件互動，Facade 只面向 Client |

---

## ✅ 使用時機

**應該用 Facade 的情況：**
- 子系統越來越複雜，Client 需要了解太多細節
- 想為程式庫或框架提供**簡單的入口**
- 想讓 Client 與子系統**低耦合**，之後替換子系統也不影響 Client
- 例：家庭劇院控制、第三方 SDK 封裝、微服務 API Gateway

**不適用的情況：**
- 子系統本身就很簡單，加 Facade 只是增加多餘的一層

---

## 🔑 一句話記憶

> **「複雜的事情我來做，你只需要告訴我你要什麼」**  
> Facade 把多個子系統的操作步驟封裝成簡單的高層方法，讓 Client 無需了解背後的複雜流程。
