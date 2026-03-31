# 責任鏈模式 (Chain of Responsibility Pattern)

**行為型設計模式 (Behavioral Design Pattern)**

責任鏈模式的核心精神是：**為了解除「請求發送者」與「請求接收者」之間的耦合關係，將多個可以處理該請求的物件串聯成一條「鏈 (Chain)」。當請求發生時，讓這個請求沿著這條鏈傳遞，直到有物件能夠處理它為止。**

## 📝 解決了什麼問題？

在沒有責任鏈的情況下，發送請求的程式碼通常需要寫一大堆 `if-else` 或 `switch` 來判斷要把請求交給誰處理，導致程式設計充滿了緊密結合（耦合）：

```csharp
// 糟糕的作法：充滿緊密耦合與龐大的 if-else
if (amount <= 1000) {
    manager.Approve(request);
} else if (amount <= 50000) {
    director.Approve(request);
} else {
    ceo.Approve(request);
}
```

使用責任鏈模式後：
* **發送者**不需要知道到底鏈條裡的哪一個節點會處理這個請求，它只需要把請求丟給「鏈的起點」即可。
* **處理者**只需要專注於「自己能處理的範圍」，如果超過自己能力，往上呈報就好（單一職責原則 SRP）。

## ⚙️ 系統結構與角色

在這個費用申請審核的範例中，參與的角色有：

1. **`Approver` (抽象處理者：Handler)**：定義出一個處理請求的抽象類別，並且包含一個指向「下一個處理者」的參考 `_nextApprover`。
2. **`Manager`, `Director`, `CEO` (具體處理者：ConcreteHandler)**：實作 `Approver`。如果自己能處理該請求，就處理掉；如果不能處理，就把請求傳遞給 `_nextApprover`。

---

## 💻 完整程式碼實作

### 1. 抽象處理者 (Approver)

定義審核的共同行為，以及負責串接下一位審核者的 `SetNext()` 方法。

```csharp
namespace DesignPatterns.Behavioral.Chain_of_Responsibility
{
    public abstract class Approver
    {
        protected Approver _nextApprover;
        
        // 設定下一位審核者，並回傳它以便支援「流暢介面支援(Fluent Interface)」串接
        public Approver SetNext(Approver nextApprover)
        {
            _nextApprover = nextApprover;
            return nextApprover;
        }

        // 處理請求的通訊協定
        public abstract void ProcessRequest(int amount);
    }
}
```

### 2. 具體處理者 (Concrete Handlers)

各級主管負責處理自己權限內的請求。

**經理 (Manager)** —— 權限：1,000 元以下
```csharp
namespace DesignPatterns.Behavioral.Chain_of_Responsibility
{
    public class Manager : Approver
    {
        public override void ProcessRequest(int amount)
        {
            if(amount <= 1000)
            {
                Console.WriteLine($"Manager 核准了 {amount}元的申請。");
            }
            else if(_nextApprover != null)
            {
                Console.WriteLine("Manager權限不足，往上呈報...");
                _nextApprover.ProcessRequest(amount);
            }
        }
    }
}
```

**部門總監 (Director)** —— 權限：50,000 元以下
```csharp
namespace DesignPatterns.Behavioral.Chain_of_Responsibility
{
    public class Director : Approver
    {
        public override void ProcessRequest(int amount)
        {
            if (amount <= 50000)
            {
                Console.WriteLine($"Director 核准了{amount}元的申請");
            }
            else if (_nextApprover != null)
            {
                Console.WriteLine("Director權限不足，往上呈報");
                _nextApprover.ProcessRequest(amount);
            }
        }
    }
}
```

**最高執行長 (CEO)** —— 權限：無上限（鏈的終點）
```csharp
namespace DesignPatterns.Behavioral.Chain_of_Responsibility
{
    public class CEO : Approver
    {
        public override void ProcessRequest(int amount)
        {
            Console.WriteLine($"CEO核准了{amount}元的申請");
        }
    }
}
```

### 3. 用戶端呼叫端 (Client : 組裝與發起請求)

使用 `Approver` 宣告變數，體現「**針對介面設計，而非針對實作設計**」的原則，讓程式具備多型(Polymorphism)優勢。

```csharp
// 1. 建立各個節點 (利用 Approver 宣告增強靈活性)
Approver manager = new Manager();
Approver director = new Director();
Approver ceo = new CEO();

// 2. 組裝責任鏈：Manager -> Director -> CEO
manager.SetNext(director).SetNext(ceo);

// 3. 發送請求 (用戶端只需要接觸責任鏈的「起點」)
Console.WriteLine("--- 申請 500 元 ---");
manager.ProcessRequest(500);

Console.WriteLine("\n--- 申請 35,000 元 ---");
manager.ProcessRequest(35000);

Console.WriteLine("\n--- 申請 1,000,000 元 ---");
manager.ProcessRequest(1000000);
```

**輸出結果：**
```text
--- 申請 500 元 ---
Manager 核准了 500元的申請。

--- 申請 35,000 元 ---
Manager權限不足，往上呈報...
Director 核准了35000元的申請

--- 申請 1,000,000 元 ---
Manager權限不足，往上呈報...
Director權限不足，往上呈報
CEO核准了1000000元的申請
```

## 💡 學習重點總結

1. **依賴反轉與多型：** 
   使用 `Approver manager = new Manager();` 符合物件導向黃金準則：「針對介面(抽象)寫程式，不要針對實作(具體)寫程式」。這確保未來要抽換或增加新的主管角色（例如 `TeamLeader`）時，依賴的方法呼叫都不用修改。
2. **動態組裝(Flexible Composition)：**
   呼叫端可以決定責任鏈如何連接。不一定要從 Manager 開始，也可以隨意排列改變職責順序（如 `director.SetNext(manager)` 雖不合常規，但架構上是支援的），完全不用更動核心邏輯。
3. **優缺點評估：**
   * **優點：** 大量減少 `if-else`，類別之間充分解耦，容易擴展新節點。
   * **缺點：** 當責任鏈變長時可能會有效能損耗，並且如果處理到了鏈的終點都沒有人處理，請求可能會被靜默拋棄（通常要在最後一個 Handler 做例外處理或預設處理）。
