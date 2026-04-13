# Flyweight 享元模式

> **分類**：結構型模式（Structural Pattern）  
> **別名**：Cache、共享模式

---

## 🎯 核心概念

**Flyweight** 的目標是：**用共享物件，支援大量細粒度物件的高效使用**。

當系統需要建立**大量相似物件**，而這些物件大部分的資料都是相同的，就可以把「相同的部分」抽出來共享，只保留「不同的部分」在每個物件上。

### 關鍵切割：內在狀態 vs. 外在狀態

| | 內在狀態 (Intrinsic State) | 外在狀態 (Extrinsic State) |
|---|---|---|
| **定義** | 物件間可共享、不隨場景改變的資料 | 隨使用場景而不同的資料 |
| **存放位置** | Flyweight 物件內部 | 由 Client 傳入，或存在 Context 物件 |
| **本例** | 樹木品種、顏色、紋理 (`TreeType`) | 每棵樹的座標 X, Y (`Tree`) |

---

## 🗺 UML 類別圖

```
┌─────────────────┐          ┌──────────────────────────────┐
│   TreeFactory   │          │         TreeType              │
│─────────────────│  creates │──────────────────────────────│
│ _treeTypes      │─────────►│ Name     (內在狀態)           │
│─────────────────│          │ Color    (內在狀態)           │
│ GetTreeType()   │          │ Texture  (內在狀態)           │
│ GetCacheSize()  │          │──────────────────────────────│
└─────────────────┘          │ Draw(x, y)                   │
                             └──────────────────────────────┘
                                           ▲
                                           │ 持有（共享）
┌──────────────┐    uses    ┌─────────────┴────────────┐
│    Forest    │───────────►│           Tree            │
│──────────────│            │───────────────────────────│
│ _trees       │            │ _x, _y  (外在狀態)        │
│──────────────│            │ _type   ──► TreeType      │
│ PlantTree()  │            │───────────────────────────│
│ Draw()       │            │ Draw()                    │
└──────────────┘            └───────────────────────────┘
```

---

## 📂 程式碼解析

### 1. `TreeType.cs` — Flyweight（內在狀態）

```csharp
public class TreeType
{
    public string Name    { get; private set; }  // 共享：品種
    public string Color   { get; private set; }  // 共享：顏色
    public string Texture { get; private set; }  // 共享：紋理

    // 外在狀態 x, y 由外部傳入，不存在物件內
    public void Draw(int x, int y)
    {
        Console.WriteLine($"繪製樹木:[品種={Name},...] 於座標({x},{y})");
    }
}
```

> **重點**：`Draw(int x, int y)` 接受外在狀態當參數，而非存在欄位中。

---

### 2. `Tree.cs` — Context（外在狀態容器）

```csharp
public class Tree
{
    private int      _x;     // 外在狀態：X 座標
    private int      _y;     // 外在狀態：Y 座標
    private TreeType _type;  // 參考共享的 Flyweight

    public void Draw() => _type.Draw(_x, _y);
}
```

> **重點**：`Tree` 只保留「這棵樹獨有的資料」，共同的 `TreeType` 以參考來共享。

---

### 3. `TreeFactory.cs` — Flyweight Factory（管理共享物件）

```csharp
public class TreeFactory
{
    // 快取池：key = "品種_顏色_紋理"
    private static Dictionary<string, TreeType> _treeTypes = new();

    public static TreeType GetTreeType(string name, string color, string texture)
    {
        string key = $"{name}_{color}_{texture}";

        if (!_treeTypes.ContainsKey(key))         // 不存在才建立
        {
            _treeTypes[key] = new TreeType(name, color, texture);
        }

        return _treeTypes[key];  // 回傳共享物件
    }
}
```

> **重點**：工廠用 Dictionary 做快取，相同種類的 `TreeType` 只建立一次。

---

## 💡 記憶體節省量化

假設遊戲中有 **100,000 棵樹**，樹的品種只有 3 種：

| 方案 | TreeType 物件數量 | 記憶體消耗 |
|---|---|---|
| **無 Flyweight** | 100,000 個 | 極大 |
| **有 Flyweight** | **3 個** | 極小 |

每個 `Tree` 只多付出一個「參考指標」（8 bytes），而不是重複儲存整份 TreeType 資料。

---

## ⚖️ 與其他模式比較

| 模式 | 目的 | 備註 |
|---|---|---|
| **Flyweight** | 減少記憶體：共享內在狀態 | 大量細粒度物件 |
| **Singleton** | 確保只有一個實例 | 只有一個全域物件 |
| **Proxy** | 控制存取 / 延遲初始化 | 一對一代理 |

> Flyweight 常和 **Factory** 搭配使用（此例的 `TreeFactory` 就是）。

---

## ✅ 使用時機

**應該用 Flyweight 的情況：**
- 系統需要建立**大量（成千上萬）相似物件**
- 物件的大部分狀態可以變為**外在狀態**（由外部傳入）
- 例：遊戲粒子、地圖磁磚、文字編輯器的字元、UI 圖示

**不適用的情況：**
- 物件數量少（共享帶來的複雜度不值得）
- 物件幾乎沒有可共享的狀態

---

## 🔑 一句話記憶

> **「相同的共享，不同的傳入」**  
> 把物件中不變的部分 (Intrinsic) 提取出來共享，把變動的部分 (Extrinsic) 當參數傳進來。
