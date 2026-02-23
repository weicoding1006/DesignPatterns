# 介面隔離原則 (Interface Segregation Principle, ISP)

**SOLID** 原則中的 **I** 代表 **介面隔離原則 (Interface Segregation Principle)**。

**核心概念**：
> "Clients should not be forced to depend upon interfaces that they do not use."
> （客戶端不應該被強迫依賴它們不使用的介面。）

簡單來說，一個類別不應該被迫實作它不需要的方法。與其建立一個龐大且包含所有功能的「胖介面 (Fat Interface)」，不如將其拆分成多個特定且功能單一的小介面。

---

## 反面教材：違反 ISP 的設計 (`BadExample.cs`)

在 `BadExample.cs` 中，我們看到了一個違反介面隔離原則的設計：

```csharp
public interface IBadMachine
{
    void Print(string content);
    void Scan(string content);
    void Fax(string content);
}
```

這是一個「胖介面」，包含了列印、掃描和傳真三種功能。

對於高階多功能事務機 `BadAdvancedMachine` 來說，實作這個介面沒有問題，因為它確實具備這三種功能。

但是，對於一台**普通印表機 (`BadBasicPrinter`)** 來說，這就成了一場災難：

```csharp
public class BadBasicPrinter : IBadMachine
{
    public void Fax(string content)
    {
        throw new NotImplementedException("不支援"); // 被迫實作不需要的方法
    }

    public void Print(string content)
    {
        Console.WriteLine(content);
    }

    public void Scan(string content)
    {
        throw new NotImplementedException("不支援"); // 被迫實作不需要的方法
    }
}
```

**違反原因：** `BadBasicPrinter` 只會列印，卻被迫依賴並實作了 `Scan` 和 `Fax`。這不僅造成程式碼的冗餘，當呼叫端不小心執行到這些方法時，還會引發 `NotImplementedException` 異常，增加了系統的不穩定性與除錯成本。

---

## 正確示範：符合 ISP 的設計 (`Example.cs`)

為了解決上述問題，在 `Example.cs` 中，我們將龐大的 `IBadMachine` 拆分成三個獨立且職責單一的小介面：

```csharp
public interface IPrinter
{
    void Print(string content);
}

public interface IScanner
{
    void Scan(string content);
}

public interface IFaxer
{
    void Fax(string content);
}
```

現在，每個類別只需要實作自己真正需要的功能：

**1. 高階多功能事務機 (`AdvancedMachine`)：**
如果支援多種功能，可以同時實作多個小介面。這展示了介面的靈活性：

```csharp
public class AdvancedMachine : IPrinter, IScanner, IFaxer
{
    // 實作 Print, Scan, Fax，所有功能都被正確提供
    // ...略...
}
```

**2. 普通印表機 (`BasicPrinter`)：**
只需要獨立實作它唯一的功能：`IPrinter`。再也不用被迫實作會拋出例外的無用方法，程式碼變得非常乾淨且直覺：

```csharp
public class BasicPrinter : IPrinter
{
    public void Print(string content)
    {
        Console.WriteLine(content);
    }
}
```

## 總結

**介面隔離原則 (ISP)** 的主要好處：
1. **降低耦合度**：類別只依賴它真正需要且會使用到的介面。
2. **提高程式碼可讀性與可維護性**：小而美的介面更容易理解與實作。
3. **避免改動帶來的連鎖反應**：介面被拆分後，如果日後 `IScanner` 新增了其他方法，只會影響實作 `IScanner` 的 `AdvancedMachine`，完全不會干擾到 `BasicPrinter` 的編譯與運作。
