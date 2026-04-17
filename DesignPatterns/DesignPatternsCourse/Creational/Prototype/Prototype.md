# Prototype 模式（原型模式）

## 1. 什麼是 Prototype 模式？

Prototype 屬於**創建型設計模式 (Creational Design Pattern)**，核心思想是：

> **「透過複製（Clone）一個現有的物件（稱為『原型』）來建立新物件，而不是每次都從頭用 `new` 重新建立。」**

這裡的關鍵詞是**「Clone（克隆）」**。Prototype 不關心物件屬於哪個類別或如何被組裝；它只關心「**我已經有一個差不多的物件，只需要複製後略加修改**」。

**沒有 Prototype 會怎樣？**

假設要建立一張「怪物配置表」。初學者若每次都用 `new + 手動 setter` 重新建立：

```csharp
// 反模式：每次建立新怪物都要重複設定大量相同欄位
var goblin1 = new Monster();
goblin1.Name       = "哥布林";
goblin1.Race       = "哥布林族";
goblin1.Size       = MonsterSize.Small;
goblin1.BaseHP     = 50;
goblin1.BaseATK    = 10;
goblin1.Resistances = new List<string> { "暗屬性" };
goblin1.Drops       = new List<string> { "哥布林耳朵", "銅幣" };
goblin1.Level = 1;
goblin1.Name  = "哥布林（Lv.1）";

var goblin2 = new Monster();
goblin2.Name       = "哥布林";
goblin2.Race       = "哥布林族";
goblin2.Size       = MonsterSize.Small;
goblin2.BaseHP     = 50;
goblin2.BaseATK    = 10;
goblin2.Resistances = new List<string> { "暗屬性" };   // ← 每次都要重建 List
goblin2.Drops       = new List<string> { "哥布林耳朵", "銅幣" };
goblin2.Level = 5;
goblin2.Name  = "哥布林菁英（Lv.5）";
```

問題顯而易見：
1. **大量重複**：欄位越多，重複的 setter 越多。
2. **漏填風險**：某個必要欄位忘了設定，執行時才爆炸。
3. **效能差**：若初始化包含讀檔、呼叫 API，每次 `new` 都是一次昂貴操作。

---

## 2. 本範例的設計：遊戲怪物生成系統

以一個 RPG 遊戲的「怪物配置系統」作為範例：

遊戲中有多種怪物（哥布林、骷髏士兵、龍），每種怪物有一個**原型（Prototype）**，代表該種族的基礎配置。當需要大量生成同類怪物時，不重新建立，而是**Clone 原型後只修改差異部分（如等級、名稱）**。

### 架構圖

```
┌──────────────────────────────────────────────────────────────┐
│               <<interface>> IMonsterPrototype                │
│  + Clone() : IMonsterPrototype                               │
└───────────────────────────┬──────────────────────────────────┘
                            │ 實作
         ┌──────────────────┼──────────────────┐
         │                  │                  │
┌────────┴───────┐  ┌───────┴────────┐  ┌──────┴──────────┐
│    Goblin      │  │ SkeletonWarrior│  │     Dragon       │
│ (具體原型 A)   │  │  (具體原型 B)  │  │  (具體原型 C)   │
│ Clone() ← 深拷貝│  │ Clone() ← 深拷貝│  │ Clone() ← 深拷貝│
└────────────────┘  └────────────────┘  └─────────────────┘

┌──────────────────────────────────────────────────────────┐
│                MonsterRegistry（原型登錄表）              │
│  - _prototypes : Dictionary<string, IMonsterPrototype>   │
│  + Register(key, prototype)                              │
│  + Spawn(key) : IMonsterPrototype    ← Clone 並回傳      │
└──────────────────────────────────────────────────────────┘
```

> **MonsterRegistry** 是「原型登錄表」，負責管理所有原型，並提供 `Spawn()` 方法——呼叫方只需說「我要一個哥布林」，Registry 就 Clone 對應原型後回傳，呼叫方完全不感知 Clone 的細節。

---

## 3. 淺拷貝 vs 深拷貝

這是 Prototype 模式中最重要的概念，**必須在實作前決定你需要哪一種**。

| | 淺拷貝 (Shallow Copy) | 深拷貝 (Deep Copy) |
|:---|:---|:---|
| **說明** | 複製物件本身的欄位值；若欄位是參考型別（如 `List<T>`），複製的是**指標**，兩個物件共用同一份資料 | 遞迴複製所有層級；內部的參考型別也會建立全新的獨立副本 |
| **風險** | 修改 Clone 物件的 `List`，**會影響原型**，造成意外 Bug | 無上述風險，但實作較複雜 |
| **C# 內建方法** | `MemberwiseClone()`（`protected`，只能在類別內部呼叫） | 手動實作（逐欄 `new List<T>(source)`）或序列化技巧 |
| **適用時機** | 物件內部沒有參考型別，或參考型別是 **immutable**（如 `string`） | 物件內部有可變的 `List`、`Dictionary`、自訂類別等 |

### 淺拷貝陷阱示範

```csharp
var original = new Monster { Drops = new List<string> { "金幣" } };
var clone    = (Monster)original.MemberwiseClone(); // 淺拷貝

clone.Drops.Add("怪物皮革");   // 修改 clone 的 Drops

// ⚠ original.Drops 也被修改了！因為兩者共用同一個 List 物件
Console.WriteLine(original.Drops.Count); // 輸出 2，不是預期的 1
```

本範例使用**深拷貝**，確保 Clone 出的怪物與原型完全獨立。

---

## 4. 程式碼

### 4.1 共用列舉與輔助型別

```csharp
// 怪物體型分類
public enum MonsterSize
{
    Small,   // 小型（哥布林）
    Medium,  // 中型（骷髏士兵）
    Large    // 大型（龍）
}
```

---

### 4.2 Prototype 介面：`IMonsterPrototype`

```csharp
// 所有可被克隆的怪物原型必須實作此介面
// Clone() 回傳自身型別，讓呼叫方不需要強制轉型
public interface IMonsterPrototype
{
    // 深拷貝：回傳一個與自身完全獨立的新物件
    IMonsterPrototype Clone();

    // 供外部快速修改差異部分（如等級、名稱）
    void SetLevel(int level);

    // 展示怪物配置
    void Show();
}
```

---

### 4.3 具體原型 A：`Goblin`（哥布林）

```csharp
// 具體原型 A：哥布林
// 示範如何在 Clone() 中實作「深拷貝」
public class Goblin : IMonsterPrototype
{
    public string      Name        { get; set; } = "哥布林";
    public string      Race        { get; set; } = "哥布林族";
    public MonsterSize Size        { get; set; } = MonsterSize.Small;
    public int         BaseHP      { get; set; } = 50;
    public int         BaseATK     { get; set; } = 10;
    public int         Level       { get; set; } = 1;

    // List<T> 是參考型別，必須深拷貝，否則 Clone 與原型共用同一份 List
    public List<string> Resistances { get; set; } = new() { "暗屬性" };
    public List<string> Drops       { get; set; } = new() { "哥布林耳朵", "銅幣" };

    // 深拷貝實作：
    // 1. 先用 MemberwiseClone() 複製所有值型別欄位（int、enum、string 本身是 immutable）
    // 2. 再手動 new 一份 List，切斷與原型的共用關係
    public IMonsterPrototype Clone()
    {
        var clone = (Goblin)MemberwiseClone();           // 淺拷貝（複製值型別）
        clone.Resistances = new List<string>(Resistances); // 深拷貝 List
        clone.Drops       = new List<string>(Drops);       // 深拷貝 List
        return clone;
    }

    public void SetLevel(int level)
    {
        Level = level;
        Name  = level >= 5
            ? $"哥布林菁英（Lv.{level}）"
            : $"哥布林（Lv.{level}）";
    }

    public void Show()
    {
        Console.WriteLine($"  ┌─── {Name} ───────────────────");
        Console.WriteLine($"  │  種族  : {Race}");
        Console.WriteLine($"  │  體型  : {Size}");
        Console.WriteLine($"  │  HP    : {BaseHP + Level * 10}  (Base {BaseHP} + Lv.{Level}×10)");
        Console.WriteLine($"  │  ATK   : {BaseATK + Level * 2}  (Base {BaseATK} + Lv.{Level}×2)");
        Console.WriteLine($"  │  抗性  : {string.Join(", ", Resistances)}");
        Console.WriteLine($"  │  掉落  : {string.Join(", ", Drops)}");
        Console.WriteLine($"  └──────────────────────────────────");
    }
}
```

---

### 4.4 具體原型 B：`SkeletonWarrior`（骷髏士兵）

```csharp
// 具體原型 B：骷髏士兵
// 示範含有「巢狀物件」時的深拷貝（Loot 是自訂類別）
public class LootTable
{
    public List<string> Items      { get; set; } = new();
    public int          GoldAmount { get; set; } = 0;

    // LootTable 自身也需要深拷貝
    public LootTable DeepCopy()
    {
        return new LootTable
        {
            Items      = new List<string>(Items),
            GoldAmount = GoldAmount
        };
    }
}

public class SkeletonWarrior : IMonsterPrototype
{
    public string      Name    { get; set; } = "骷髏士兵";
    public string      Race    { get; set; } = "不死族";
    public MonsterSize Size    { get; set; } = MonsterSize.Medium;
    public int         BaseHP  { get; set; } = 80;
    public int         BaseATK { get; set; } = 18;
    public int         Level   { get; set; } = 1;

    // 骷髏士兵有一個巢狀自訂物件（比 Goblin 更複雜的深拷貝情境）
    public List<string> Resistances { get; set; } = new() { "物理減傷 30%", "不死免疫" };
    public LootTable    Loot        { get; set; } = new()
    {
        Items      = new List<string> { "骨頭", "生鏽短劍" },
        GoldAmount = 15
    };

    public IMonsterPrototype Clone()
    {
        var clone = (SkeletonWarrior)MemberwiseClone();
        clone.Resistances = new List<string>(Resistances);
        clone.Loot        = Loot.DeepCopy();             // 巢狀物件也深拷貝
        return clone;
    }

    public void SetLevel(int level)
    {
        Level = level;
        Name  = level >= 10
            ? $"骷髏騎士（Lv.{level}）"
            : $"骷髏士兵（Lv.{level}）";

        // 高等級才有稀有掉落
        if (level >= 10 && !Loot.Items.Contains("古代劍"))
            Loot.Items.Add("古代劍");
    }

    public void Show()
    {
        Console.WriteLine($"  ┌─── {Name} ───────────────────");
        Console.WriteLine($"  │  種族  : {Race}");
        Console.WriteLine($"  │  體型  : {Size}");
        Console.WriteLine($"  │  HP    : {BaseHP + Level * 15}  (Base {BaseHP} + Lv.{Level}×15)");
        Console.WriteLine($"  │  ATK   : {BaseATK + Level * 3}  (Base {BaseATK} + Lv.{Level}×3)");
        Console.WriteLine($"  │  抗性  : {string.Join(", ", Resistances)}");
        Console.WriteLine($"  │  掉落  : {string.Join(", ", Loot.Items)} + {Loot.GoldAmount} 金幣");
        Console.WriteLine($"  └──────────────────────────────────");
    }
}
```

---

### 4.5 具體原型 C：`Dragon`（龍）

```csharp
// 具體原型 C：龍（BOSS 等級）
// 展示多層 List 的深拷貝，以及原型攻擊技能組的複製
public class Dragon : IMonsterPrototype
{
    public string      Name    { get; set; } = "古龍";
    public string      Race    { get; set; } = "龍族";
    public MonsterSize Size    { get; set; } = MonsterSize.Large;
    public int         BaseHP  { get; set; } = 500;
    public int         BaseATK { get; set; } = 80;
    public int         Level   { get; set; } = 1;

    public List<string> Resistances { get; set; } = new() { "火屬性免疫", "物理減傷 50%" };
    public List<string> Skills      { get; set; } = new() { "爪擊", "烈焰吐息", "尾掃" };
    public List<string> Drops       { get; set; } = new() { "龍鱗", "龍牙", "龍心石" };

    public IMonsterPrototype Clone()
    {
        var clone = (Dragon)MemberwiseClone();
        clone.Resistances = new List<string>(Resistances);
        clone.Skills      = new List<string>(Skills);
        clone.Drops       = new List<string>(Drops);
        return clone;
    }

    public void SetLevel(int level)
    {
        Level = level;
        Name  = level >= 50
            ? $"神話古龍（Lv.{level}）"
            : $"古龍（Lv.{level}）";

        // 高等級龍解鎖額外技能
        if (level >= 30 && !Skills.Contains("龍息衝波"))
            Skills.Add("龍息衝波");
        if (level >= 50 && !Skills.Contains("世界終焉"))
            Skills.Add("世界終焉");
    }

    public void Show()
    {
        Console.WriteLine($"  ┌─── {Name} ───────────────────");
        Console.WriteLine($"  │  種族  : {Race}");
        Console.WriteLine($"  │  體型  : {Size}");
        Console.WriteLine($"  │  HP    : {BaseHP + Level * 50}  (Base {BaseHP} + Lv.{Level}×50)");
        Console.WriteLine($"  │  ATK   : {BaseATK + Level * 10}  (Base {BaseATK} + Lv.{Level}×10)");
        Console.WriteLine($"  │  抗性  : {string.Join(", ", Resistances)}");
        Console.WriteLine($"  │  技能  : {string.Join(", ", Skills)}");
        Console.WriteLine($"  │  掉落  : {string.Join(", ", Drops)}");
        Console.WriteLine($"  └──────────────────────────────────");
    }
}
```

---

### 4.6 原型登錄表：`MonsterRegistry`

```csharp
// 原型登錄表（Prototype Registry）
// 負責管理所有原型，並提供 Spawn() 方法統一對外服務
// 呼叫方只需提供 key 字串，不需要感知任何具體怪物類別
public class MonsterRegistry
{
    // key = 怪物種類識別字串，value = 原型物件
    private readonly Dictionary<string, IMonsterPrototype> _prototypes = new();

    // 將一個原型物件登錄到 Registry
    public void Register(string key, IMonsterPrototype prototype)
    {
        _prototypes[key] = prototype;
        Console.WriteLine($"  [Registry] 已登錄原型：{key}");
    }

    // Clone 對應原型並回傳，呼叫方取得的是獨立的新物件
    // 若 key 不存在則拋出例外，讓問題在開發期間提早暴露
    public IMonsterPrototype Spawn(string key)
    {
        if (!_prototypes.TryGetValue(key, out var prototype))
            throw new KeyNotFoundException($"找不到怪物原型：'{key}'，請先呼叫 Register() 登錄");

        return prototype.Clone(); // ← 核心：Clone 而非 new
    }
}
```

> **為什麼要有 Registry？**
> Registry 扮演「原型工廠」的角色：呼叫方不需要持有任何具體原型物件，也不需要知道 `Goblin`、`Dragon` 等類別的存在，只需要知道字串 key。這讓新增怪物類型時，呼叫方的程式碼完全不需要修改，滿足**開閉原則（OCP）**。

---

## 5. 參與的角色

| 角色 | 本範例對應 | 職責 |
|:---|:---|:---|
| **Prototype（介面）** | `IMonsterPrototype` | 宣告 `Clone()` 方法，所有可被克隆的物件都實作此介面 |
| **ConcretePrototype** | `Goblin`、`SkeletonWarrior`、`Dragon` | 實作 `Clone()` 的具體深拷貝邏輯 |
| **Registry（選用）** | `MonsterRegistry` | 管理原型的字典，提供 `Spawn(key)` 作為統一的 Clone 入口 |
| **Client（呼叫方）** | `Program.cs` | 向 Registry 取得 Clone，修改差異部分後使用 |

---

## 6. 如何在 Program.cs 測試執行

```csharp
Console.WriteLine("=== Prototype Pattern ===");

// ── 步驟 1：建立原型登錄表，並登錄各種原型 ──
Console.WriteLine("\n[ 步驟 1：初始化 MonsterRegistry，登錄三種怪物原型 ]");
var registry = new MonsterRegistry();
registry.Register("goblin",   new Goblin());
registry.Register("skeleton", new SkeletonWarrior());
registry.Register("dragon",   new Dragon());

// ── 步驟 2：Spawn 哥布林（展示淺拷貝 vs 深拷貝的安全性）──
Console.WriteLine("\n[ 步驟 2：Spawn 多隻哥布林並設定不同等級 ]");
var goblinLv1  = (Goblin)registry.Spawn("goblin");
goblinLv1.SetLevel(1);

var goblinLv5  = (Goblin)registry.Spawn("goblin");
goblinLv5.SetLevel(5);
goblinLv5.Drops.Add("菁英勳章");   // ← 只修改 clone，不影響原型

var goblinLv10 = (Goblin)registry.Spawn("goblin");
goblinLv10.SetLevel(10);
goblinLv10.Drops.Add("哥布林頭盔");

goblinLv1.Show();
goblinLv5.Show();
goblinLv10.Show();

// ── 步驟 3：Spawn 骷髏士兵（含巢狀物件 LootTable）──
Console.WriteLine("\n[ 步驟 3：Spawn 骷髏士兵（巢狀物件深拷貝） ]");
var skeleton1 = (SkeletonWarrior)registry.Spawn("skeleton");
skeleton1.SetLevel(5);

var skeleton2 = (SkeletonWarrior)registry.Spawn("skeleton");
skeleton2.SetLevel(12);    // 升級成骷髏騎士，會自動加上 "古代劍" 掉落

skeleton1.Show();
skeleton2.Show();

// ── 步驟 4：Spawn 龍（BOSS）──
Console.WriteLine("\n[ 步驟 4：Spawn 龍（高等級解鎖技能） ]");
var dragonLv20 = (Dragon)registry.Spawn("dragon");
dragonLv20.SetLevel(20);

var dragonLv55 = (Dragon)registry.Spawn("dragon");
dragonLv55.SetLevel(55);   // 解鎖「龍息衝波」與「世界終焉」

dragonLv20.Show();
dragonLv55.Show();

// ── 步驟 5：驗證深拷貝的獨立性 ──
Console.WriteLine("\n[ 步驟 5：驗證深拷貝——修改 Clone 不影響原型 ]");
var original = new Goblin();
var clone    = (Goblin)original.Clone();
clone.Drops.Add("測試道具");   // 只加到 clone 的 Drops

Console.WriteLine($"  原型 Drops 數量 : {original.Drops.Count}  （應為 2，不受影響）");
Console.WriteLine($"  Clone  Drops 數量 : {clone.Drops.Count}  （應為 3，獨立）");
```

**預期輸出：**
```
=== Prototype Pattern ===

[ 步驟 1：初始化 MonsterRegistry，登錄三種怪物原型 ]
  [Registry] 已登錄原型：goblin
  [Registry] 已登錄原型：skeleton
  [Registry] 已登錄原型：dragon

[ 步驟 2：Spawn 多隻哥布林並設定不同等級 ]
  ┌─── 哥布林（Lv.1） ───────────────────
  │  種族  : 哥布林族
  │  體型  : Small
  │  HP    : 60  (Base 50 + Lv.1×10)
  │  ATK   : 12  (Base 10 + Lv.1×2)
  │  抗性  : 暗屬性
  │  掉落  : 哥布林耳朵, 銅幣
  └──────────────────────────────────
  ┌─── 哥布林菁英（Lv.5） ───────────────────
  │  種族  : 哥布林族
  │  體型  : Small
  │  HP    : 100  (Base 50 + Lv.5×10)
  │  ATK   : 20  (Base 10 + Lv.5×2)
  │  抗性  : 暗屬性
  │  掉落  : 哥布林耳朵, 銅幣, 菁英勳章
  └──────────────────────────────────
  ...（以此類推）

[ 步驟 5：驗證深拷貝——修改 Clone 不影響原型 ]
  原型 Drops 數量 : 2  （應為 2，不受影響）
  Clone  Drops 數量 : 3  （應為 3，獨立）
```

---

## 7. 兩種 Clone 實作策略比較

### 7.1 策略一：MemberwiseClone + 手動深拷貝（本範例採用）

```csharp
public IMonsterPrototype Clone()
{
    var clone = (Goblin)MemberwiseClone();      // 快速複製值型別
    clone.Resistances = new List<string>(Resistances); // 手動深拷貝 List
    clone.Drops       = new List<string>(Drops);
    return clone;
}
```

**優點：** 效能最好，不依賴序列化；可精細控制每個欄位的複製策略  
**缺點：** 增加欄位時需記得更新 `Clone()` 方法，容易遺漏

---

### 7.2 策略二：JSON 序列化（適合快速開發）

```csharp
// 需要 System.Text.Json 或 Newtonsoft.Json
public IMonsterPrototype Clone()
{
    var json  = JsonSerializer.Serialize(this);
    return JsonSerializer.Deserialize<Goblin>(json)!;
}
```

**優點：** 自動深拷貝所有層級，不怕遺漏欄位  
**缺點：** 效能較差；欄位需可序列化；型別資訊可能遺失（多型場景要注意）

---

### 7.3 策略三：複製建構子（Copy Constructor）

```csharp
// 複製建構子是 Prototype 的另一種實作形式
public Goblin(Goblin other)
{
    Name        = other.Name;
    Race        = other.Race;
    Size        = other.Size;
    BaseHP      = other.BaseHP;
    BaseATK     = other.BaseATK;
    Level       = other.Level;
    Resistances = new List<string>(other.Resistances);
    Drops       = new List<string>(other.Drops);
}

// 使用方式
var clone = new Goblin(original);
```

**優點：** 語意清晰，在建構子中完整記錄所有欄位；不依賴反射或序列化  
**缺點：** 需多維護一個建構子；與 `ICloneable` 介面規範較脫節

---

## 8. ICloneable vs 自訂 Clone 介面

C# 標準函式庫內建 `ICloneable` 介面：

```csharp
// BCL 定義的 ICloneable
public interface ICloneable
{
    object Clone(); // 回傳 object，呼叫方必須強制轉型
}
```

**本範例改用自訂 `IMonsterPrototype.Clone()` 的原因：**

| | `ICloneable` | 自訂 `IMonsterPrototype` |
|:---|:---|:---|
| 回傳型別 | `object`（需轉型） | `IMonsterPrototype`（語意明確） |
| 淺 / 深拷貝語意 | 未定義（文件說「淺或深均可」）| 由介面文件明確規範為深拷貝 |
| Microsoft 官方建議 | 不建議在新 API 使用 `ICloneable` | 建議自訂語意明確的 Clone 方法 |

> **結論：** 在新專案中，建議自訂 Clone 方法（無論是透過介面或直接定義在類別上），避免使用語意模糊的 `ICloneable`。

---

## 9. Prototype vs 其他創建型模式

| | Prototype | Factory Method | Abstract Factory | Builder |
|:---|:---|:---|:---|:---|
| **建立起點** | **已有物件**（從現有物件 Clone） | 無（從無到有，由子類別決定） | 無（從無到有，一族相關物件） | 無（從無到有，多步驟組裝） |
| **核心問題** | 建立成本高 / 物件差異小 | 讓子類別決定建立哪種產品 | 確保產品家族一致性 | 複雜的可選步驟/大量參數 |
| **與具體類別的耦合** | **最低**（呼叫方只看 Clone，不感知具體類別） | 中（子類別耦合產品類別） | 中 | 中 |
| **典型應用** | 遊戲怪物生成<br>設定快照複製<br>物件快取 | `IHttpClientFactory` | UI 元件家族 | `WebApplicationBuilder` |

---

## 10. 在 ASP.NET Core Web API 的應用

| 場景 | Prototype 概念的對應 |
|:---|:---|
| **`IOptionsSnapshot<T>`** | 每次請求都從「原型設定」快照 Clone 一份，確保請求隔離且設定可熱更新 |
| **物件 Pool（`ObjectPool<T>`）** | 從 Pool 取出物件時，實質上是取用預先建立的「原型實例」，用完歸還重設 |
| **DTOs 的 AutoMapper / Mapster** | `mapper.Map<Target>(source)` 本質上是根據規則 Clone 一份新物件 |
| **快取的 DeepCopy 策略** | 從快取讀出後做 Clone 再回傳，防止使用方修改到快取內的共享物件 |
| **測試 Fixture 建立** | 用一個標準的「原型物件」，在每個測試 Case 中 Clone 後改幾個欄位，避免重複建立複雜物件 |

---

## 11. 優缺點

### ✅ 優點

- **降低建立成本**：物件初始化昂貴（讀取設定、連線外部服務），Clone 比重新 `new` 快很多。
- **降低子類別數量**：不需要為每一種「變體」建立新子類別，只需 Clone 後修改差異欄位。
- **與具體類別解耦**：呼叫方不需要知道物件的具體類別，只需呼叫 `Clone()`。
- **複雜物件預設值**：把常用的「預設組合」存為原型，相當於建立「物件模板」。

### ❌ 缺點

- **深拷貝實作複雜**：當物件有多層巢狀結構時，深拷貝的實作容易出錯，且增加欄位時需同步維護 `Clone()` 方法。
- **循環參考問題**：物件若有循環參考（A 持有 B，B 又持有 A），深拷貝會進入無窮遞迴，需要特殊處理。
- **語意不清晰**：`MemberwiseClone()` 的淺拷貝行為容易造成誤用，需要良好的文件或介面規範明確說明是深或淺拷貝。

---

## 12. 何時選用 Prototype？

```
你是否「已經有一個差不多的物件」？
│
├─ YES → 新物件與現有物件的差異是否很小？
│         ├─ YES → Prototype（Clone 後修改差異部分）
│         └─ NO  → 考慮 Builder 或 Factory 從頭建立
│
└─ NO  → 物件建立是否昂貴（讀檔、連線、複雜計算）？
          ├─ YES → 可建立一個初始化好的「原型」存在 Registry，之後都 Clone
          └─ NO  → 直接 new 即可，不需要 Prototype
```

---

## 13. 小結

```
Prototype 解決的核心問題：
┌───────────────────────────────────────────────────────────┐
│  「我需要大量相似物件，但每次 new + 手動設定太繁瑣，        │
│   而且物件初始化成本很高，我不想一直重複這個過程」          │
│                                                           │
│  ConcretePrototype → 實作 Clone()，知道自己該如何被複製    │
│  Registry          → 管理原型字典，統一提供 Spawn 入口      │
│  Client            → 只呼叫 Spawn / Clone，不 new 具體類別 │
└───────────────────────────────────────────────────────────┘
```

| | 選擇的時機 |
|---|---|
| **Prototype** | 已有差不多的物件，Clone 後略加修改；物件建立成本高 |
| **Builder** | 物件建構步驟複雜，且有幾種「有名字的標準組合」 |
| **Factory Method** | 需要建立單一物件，但由子類別決定具體類別 |
| **Abstract Factory** | 需要一次建立「一族相關物件」，確保風格一致 |
