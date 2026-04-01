# 訪問者模式 (Visitor Pattern) 

## 1. 簡介 (Introduction)
**訪問者模式 (Visitor Pattern)** 屬於**行為型設計模式 (Behavioral Design Pattern)**。它的核心思想是：**將「資料結構」與「資料操作」分離**。

這模式能讓你能在「不修改現有資料結構（類別）的情況下，定義並增加作用於這些物件結構上的新操作（新行為）」。在實際應用中，當你的物件結構（模型類別）非常穩定且沒有太多變更，但你卻需要頻繁替這組物件加入不同的操作行為時，就非常適合使用 Visitor 模式。

### 為什麼要有 Visitor 模式？
假設公司內部有 `Engineer` 和 `Manager` 兩種角色，一開始只需要基本屬性。後來需求變更，需要「計算獎金」。如果你直接在 `Engineer` 及 `Manager` 實作 `CalculateBonus()`，之後若又臨時要「計算特休天數」，你又需要去這兩種類別加上 `CalculateVacation()`。這會導致模型類別越來越龐大且違反**單一職責原則 (SRP)** 與**開閉原則 (OCP)**。

為了解決這個問題，就可以透過抽出 **訪問者 (Visitor)**，把計算邏輯搬到各別的訪問者內，維持原始的 Model 乾淨不變。

---

## 2. 核心元素結構 (Structure)
依據當前專案，我們的角色對應為：
1. **Element Interface (物件元素介面)**: `IEmployee.cs`
   - 負責定義 `Accept(IVisitor visitor)` 方法，讓物件可以接收被訪問。
2. **Concrete Elements (具體物件元素)**: `Engineer.cs`、`Manager.cs`
   - 這些就是我們系統中穩定的資料結構。必須實作 `Accept` 方法，通常內容固定為：`visitor.Visit(this);`，也就是把自身的實體當作參數傳回給 Visitor 去做處理（這稱為**雙重分派 Double Dispatch** 機制）。
3. **Visitor Interface (訪問者介面)**: `IVisitor.cs`
   - 定義一組可以拜訪各種不同物件元素 (Engineer / Manager) 的 `Visit` 方法。
4. **Concrete Visitors (具體的訪問者)**: `BonusVisitor.cs`、`VacationVisitor.cs`
   -  將真實的業務邏輯寫在這裡面，例如分別實作 `BonusVisitor` 來專心算獎金，實作 `VacationVisitor` 專心算特休。

---

## 3. 程式碼架構與解析 (Code Explain)

### (A) 介面定義 (`interface/IEmployee.cs`, `interface/IVisitor.cs`)
```csharp
namespace DesignPatterns.Behavioral.Visitor
{
    // 所有元素需要實作 IEmployee
    public interface IEmployee
    {
        // 核心方法：接收訪問者
        void Accept(IVisitor visitor);
        string Name { get; }
        int Salary { get; }
    }

    // 所有業務邏輯(Visitor)需要實作 IVisitor
    public interface IVisitor
    {
        // 針對各種具體的類別實作重載的 Visit 方法
        void Visit(Engineer engineer);
        void Visit(Manager manager);
    }
}
```

### (B) 具體 Employee 實作 (`Engineer.cs`, `Manager.cs`)
具體類別將自己的職責範圍設定好後，只需要在 `Accept` 當中呼叫 `visitor.Visit(this)` 即可。
```csharp
public class Engineer : IEmployee
{
    public string Name { get; }
    public int Salary { get; }
    public int CodesWritten { get; }

    public Engineer(string name, int salary, int codesWritten)
    {
        Name = name;
        Salary = salary;
        CodesWritten = codesWritten;
    }

    public void Accept(IVisitor visitor)
    {
        // 關鍵：將 this 交給 visitor
        visitor.Visit(this);
    }
}
```

> **⭐ `this` 在這裡代表什麼？為什麼要傳 `this`？（雙重分派機制）**
> - **`this` 代表誰**：在這裡，`this` 就是指正在執行 `Accept` 的物件實體本身（例如：「某一位特定的 Engineer」）。
> - **為什麼要傳？ (雙重分派)**：當我們在迴圈對集合中的 `IEmployee` 呼叫 `Accept` 時，程式只知道他們是「一般的員工」，不知道具體身分為何。因此我們必須請他們**主動把自己 (`this`) 丟給 Visitor**。
> - 當 `visitor.Visit(this)` 在這被執行時，因為這裡的 `this` 毫不含糊地是一個 `Engineer` 型別，C# 就會精準地去配對並呼叫 `BonusVisitor` 中專為工程師設計的 `Visit(Engineer engineer)` 方法。
> - **白話文比喻**：把 Visitor 當成醫生，`Accept` 就像看病。這裡等同於病人主動跟醫生說：「醫生（`visitor`），這是我本人（`this`），請拿我的身體資料來對我進行檢查和計算吧！」。

*(Manager 程式碼邏輯相似)*

### (C) 具體 Visitor 實作 (`BonusVisitor.cs`, `VacationVisitor.cs`)
將邏輯完全抽離放到這些具體的訪問者：
```csharp
// 獎金訪問者 - 集中處理與獎金有關的邏輯
public class BonusVisitor : IVisitor
{
    public void Visit(Engineer engineer)
    {
        double bonus = engineer.Salary * 1.5 + engineer.CodesWritten * 0.1;
        Console.WriteLine($"{engineer.Name}(工程師)的獎金是:{bonus}");
    }

    public void Visit(Manager manager)
    {
        double bonus = manager.Salary * 2 + manager.Subordinates * 5000;
        Console.WriteLine($"{manager.Name}(Manager)的獎金是:{bonus}");
    }
}
```

---

## 4. 使用方式 (Usage Example)

可以在 `Program.cs` 裡面輕鬆抽換你想套用的 Visitor 來執行各種計算。

```csharp
using System;
using System.Collections.Generic;
using DesignPatterns.Behavioral.Visitor;

// 1. 建立固定的員工名單 (資料結構)
List<IEmployee> employees = new List<IEmployee>
{
    new Engineer("Alice", 60000, 10000),
    new Engineer("Bob", 50000, 8000),
    new Manager("Hani", 100000, 3), // 注意 Namespace 若有衝突可寫全名
};

// 2. 建立具體執行的操作 (訪問者)
IVisitor bonusVisitor = new BonusVisitor();
IVisitor vacationVisitor = new VacationVisitor();

// 3. 執行操作
Console.WriteLine("--年終獎金--");
foreach(var employee in employees)
{
    // 對每位員工應用 "獎金訪問者"
    employee.Accept(bonusVisitor);
}

Console.WriteLine("\n--- 計算特休天數 ---");
foreach(var employee in employees)
{
    // 對每位員工應用 "特休訪問者"
    employee.Accept(vacationVisitor);
}
```

### 輸出結果 (Execution Output)
```text
--年終獎金--
Alice(工程師)的獎金是:91000
Bob(工程師)的獎金是:75800
Hani(Manager)的獎金是:215000

--- 計算特休天數 ---
Alice(工程師)有14天特休
Bob(工程師)有14天特休
Hani(Manager)有21天特休
```

---

## 5. 優缺點評估

### 👍 優點
1. **符合開閉原則 (OCP)**: 新增一個操作行為（例如新的 `PromoVisitor`），只需新增類別，完全不需要改動 `Engineer` 和 `Manager` 類別內部的原始碼。
2. **符合單一指責原則 (SRP)**: 相同業務的操作（例如算獎金）會被集中放在同一個 `Visitor` 類別內，不必散落各處。

### 👎 缺點
1. **增加具體元素 (Element) 很困難**: 若公司出現新職位例如 `HR` (實作 `IEmployee`)，所有的 `IVisitor` 以及其底下所有子類 (`BonusVisitor`, `VacationVisitor`) 就立刻被強制要求追加實作 `Visit(HR hr)`，牽扯規模廣大。
2. **破壞物件封裝**: 為了讓 `Visitor` 順利運算，具體元素通常會被迫公開不必要的內部狀態和細節（例如 `Salary`、`CodesWritten`）。這違反了封裝的隱密性精神。
