# 模板方法模式 (Template Method Pattern)

## 模式定義
「模板方法模式」是一種行為型設計模式。它在一個父類別的方法中定義了演算法的「骨架（流程步驟）」，並允許將其中某些具體的步驟延遲到子類別中去實作。這樣一來，子類別可以在不改變演算法核心結構的情況下，重新定義某些特定步驟的行為。

## 為什麼要使用模板方法？
在我們撰寫程式時，經常會遇到「流程幾乎一樣，但少數細微實作不同」的狀況。如果每個類別都重寫一次相同的流程，會產生大量重複的程式碼（違反 DRY 原則）。透過模板方法，我們可以把共用的流程提取到父類別中統一管理，只留下差異化的步驟讓子類別各自實現。

---

## 程式碼解析

在這次的範例中，我們以「泡飲料」為例。不管是泡咖啡還是泡茶，都有一套標準的流程：
1. 煮沸開水 `BoilWater()`
2. 沖泡（咖啡粉 / 茶葉） `Brew()`
3. 倒進杯子 `PourInCup()`
4. 加入調味料（糖與牛奶 / 檸檬） `AddCondiments()`

### 1. 定義演算法骨架：`Beverage.cs` (抽象類別)

這是一個抽象類別，它定義了泡飲料的標準流程 `PrepareRecipe()`。這就是我們的「模板方法」。

```csharp
public abstract class Beverage
{
    // 模板方法：規定了泡飲料的標準流程 (SOP)
    public void PrepareRecipe()
    {
        BoilWater();
        Brew();
        PourInCup();

        // 鉤子 (Hook)：根據條件決定是否執行附加步驟
        if(CustomerWantsCondiments())
        {
            AddCondiments();
        }
    }

    // 共用步驟：所有飲料都一樣，直接在父類別實作完畢，並宣告為 private 避免外部或子類別誤用。
    private void BoilWater()
    {
        Console.WriteLine("把水煮沸");
    }

    private void PourInCup()
    {
        Console.WriteLine("倒進杯子裡");
    }

    // 差異步驟：宣告為 abstract，強制子類別一定要各自實作對應的細節
    protected abstract void Brew();
    protected abstract void AddCondiments();

    // 鉤子方法 (Hook)：提供一個預設行為，子類別視需要（可選）覆寫此方法來影響流程
    protected virtual bool CustomerWantsCondiments()
    {
        return false; // 預設不加調味料
    }
}
```

* **共用的步驟** (`BoilWater`, `PourInCup`) 直接就在這裡寫好，免去重複代碼。
* **特定的步驟** (`Brew`, `AddCondiments`) 宣告為 `abstract` 讓子類別負責。
* **鉤子方法** (`CustomerWantsCondiments`) 是一個宣告為 `virtual` 的控制點，這可以讓子類別有能力「反過來」影響父類別流程裡的分支結構。預設是直接回傳 `false` 也就是不加調味料。

### 2. 實作具體細節：`Coffee.cs` & `Tea.cs` (實體類別)

繼承 `Beverage` 並補齊剛剛宣告為 `abstract` 的方法，且視情況覆寫 `virtual` 的鉤子方法。

#### 咖啡 (`Coffee.cs`)
```csharp
public class Coffee : Beverage
{
    // 實作咖啡專屬的沖泡方式
    protected override void Brew()
    {
        Console.WriteLine("用沸水沖泡咖啡粉");
    }

    // 實作咖啡專屬的調味料
    protected override void AddCondiments()
    {
        Console.WriteLine("加入糖和牛奶");
    }

    // 覆寫鉤子 (Hook)：因為咖啡預設要加調味料，所以回傳 true
    protected override bool CustomerWantsCondiments()
    {
        return true;
    }
}
```

#### 茶 (`Tea.cs`)
```csharp
public class Tea : Beverage
{
    protected override void Brew()
    {
        Console.WriteLine("用沸水浸泡茶葉");
    }
    
    protected override void AddCondiments()
    {
        Console.WriteLine("加入檸檬");
    }

    // 注意：茶類別並沒有覆寫 CustomerWantsCondiments()！
}
```

> **⭐️ Highlight 執行結果的奧妙：**
> 如果你看一下剛才終端機 `dotnet run` 的輸出結果，會發現泡茶時 **「加入檸檬」並沒有被印出來**！
> 這是因為 `Tea` 類別沒有覆寫 `CustomerWantsCondiments()` 這個鉤子方法，所以它沿用了 `Beverage` 父類別的預設值 (`return false`)，從而在執行 `PrepareRecipe()` 的流程時，成功繞過了加入調味料的階段。這就是 Hook (鉤子) 的經典應用！

---

## 總結

透過這三個類別，我們成功應用了模板方法模式！

* **依賴反轉（好萊塢原則）**：以往是由子類別去呼叫父類別；而在模板方法中，是由父類別的 `PrepareRecipe()` 在掌控全局流程，並主動於特定的時機點「呼叫」子類別實作的 `Brew()` 和 `AddCondiments()`（Don't call us, we'll call you）。
* **極佳的擴展性**：如果我們未來要新增一種「熱可可」，只要新建一個 `HotCocoa` 類別並繼承 `Beverage`，實作 `Brew` 和 `AddCondiments` 即可，不用怕開發者不小心改壞了原本「泡飲料」的安全順序邏輯。
