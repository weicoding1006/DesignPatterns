# Bridge 模式 (Bridge Pattern)

## 1. 什麼是 Bridge 模式？

Bridge (橋接模式) 屬於**結構型設計模式** (Structural Design Pattern)。它的核心思想是：
**「將抽象部分 (Abstraction) 與它的實作部分 (Implementation) 分離，讓它們可以各自獨立地變化。」**

這裡說的「抽象」與「實作」不單純只是 C# 語法上的 `abstract class` 或 `interface`。
- **抽象部分 (Abstraction)**：指高層次的控制邏輯與商業邏輯。
- **實作部分 (Implementation)**：指底層平台、資料庫或具體元件的基礎操作。

如果沒有使用 Bridge 模式，你可能會面臨**維度爆炸（類別數量呈倍數暴增）**的問題。

**情境說明：**
假設你要設計一個通知系統，有兩種訊息：「一般訊息」、「緊急訊息」。
起初你可能會建立 `NormalMessage` 與 `UrgentMessage`。
後來，你需要支援 **Email** 與 **SMS(簡訊)** 發送。
如果你用繼承來做，你就會得到：`EmailNormalMessage`, `EmailUrgentMessage`, `SmsNormalMessage`, `SmsUrgentMessage` (共 $2 \times 2 = 4$ 個類別)。
如果現在多了一種類型叫「極緊急訊息」，又多了一種管道叫「Line 通知」，類別就會變成 $3 \times 3 = 9$ 個。

**Bridge 的解法：將「訊息類型 (抽象)」與「發送信道 (實作)」拆開！**
讓 `Message` (抽象) 去「組合/持有」一個 `MessageSender` (實作)。兩條線各自發展，再透過 Bridge 組合在一起。這樣未來不管加幾個通知管道或訊息種類，都只要新增單一類別即可。

## 2. 參與的角色

1. **Abstraction (抽象層)**：定義高層控制邏輯 (如範例中的 `Message` 類別)，內部持有一個指向 `Implementor` 的參考。
2. **RefinedAbstraction (擴充抽象層)**：實作特定的高層邏輯 (如 `NormalMessage`, `UrgentMessage`)。
3. **Implementor (實作層介面)**：定義底層操作的介面 (如 `IMessageSender`)。
4. **ConcreteImplementor (具體實作者)**：真正實作底層邏輯的一方 (如 `EmailSender`, `SmsSender`)。

---

## 3. Web API 領域的應用：何時會用到 Bridge？

其實，如果您曾經使用現代框架（如 ASP.NET Core）開發過 Web API，您幾乎**每天都在使用或感受 Bridge 模式的設計理念**。
最常見的場景就是搭配 **依賴注入 (DI - Dependency Injection)** 來分離業務邏輯與底層設施。

### 常見情境 1：Controller / Service 層 與 Repository / DB 連線
- **Abstraction (抽象層/業務層)**：你的 `OrderService` (負責處理建立訂單、驗證商品庫存等高層邏輯)。
- **Implementor (實作介面)**：`IOrderRepository` (告訴系統底層要如何新增一筆資料)。
- **ConcreteImplementor (具體實作)**：`SqlOrderRepository` 或 `MongoDbOrderRepository`。
在這個架構中，Service 包含了特定的商業規則，而資料的存取被橋接到了介面上。今天無論你的資料庫從 SQL Server 換成 MongoDB，或者你要增加 `PremiumOrderService`，兩邊都可以**完全獨立地修改擴充，而不互相干擾**。

### 常見情境 2：金流支付系統整合 (Payment Gateway)
若你的 Web API 提供商品結帳功能：
- **抽象層**：`ProductCheckoutProcess`, `SubscriptionCheckoutProcess` (這是一次性買斷還是訂閱制？高層邏輯)。
- **實作層**：`IPaymentProvider` (處理底層呼叫 API 的邏輯)。
- **具體實作**：`StripePaymentProvider`, `LinePayProvider`, `GreenWorldPaymentProvider` (綠界)。
當客戶在前端選擇 LinePay 買斷制時，你的 Web API 在背後會建立一個 `ProductCheckoutProcess` (擴充抽象) 並把 `LinePayProvider` (具體實作) 塞給它橋接使用。

### 常見情境 3：紀錄 Log (Logging 系統)
ASP.NET Core 內建的 `ILogger` 就是一種 Bridge 的概念。
應用程式 (Controller 等) 是抽象高層，你只需呼叫 `_logger.LogInformation(...)`。
底層實作則可以各自發展，例如 `ConsoleLoggerProvider`、`FileLoggerProvider`，或是第三方的 Serilog 等。它讓「寫 Log 的高層意圖」與「實際寫入硬碟或傳送上雲的底層細節」徹底分離。

---

## 4. 如何在 Program.cs 測試執行？

請將底下程式碼放進您的 `Program.cs` 進行測試：

```csharp
using DesignPatterns.DesignPatternsCourse.Structural.Bridge;
using System;

Console.WriteLine("=== Bridge Pattern ===");

// 1. 建立底層實作者 (各種發送管道)
IMessageSender emailSender = new EmailSender();
IMessageSender smsSender = new SmsSender();

// 2. 建立高層抽象端並進行【橋接】 (一般訊息 & 緊急訊息)
Message normalEmailMessage = new NormalMessage(emailSender, "您的訂單已成立，預計三日內出貨。");
Message urgentSmsMessage = new UrgentMessage(smsSender, "伺服器 CPU 使用率飆高至 95%！");

// 3. 呼叫高層方法。他們會各自依賴其內部注入好的底層實作者完成任務
normalEmailMessage.Send();
urgentSmsMessage.Send();

/*
預期輸出:
[Email 發送] 標題: 【一般通知】 | 內容: 您的訂單已成立，預計三日內出貨。
[SMS 簡訊發送] 標題: 【緊急通知】 | 內容: !!! 緊急情況 !!! 伺服器 CPU 使用率飆高至 95%！
*/
```

## 5. 優缺點

### 優點
- **極致解耦**：分離了高度抽象的邏輯與底層細節，開發者可以並行開發這兩個層級，而不需要彼此綁死。
- **單一職責原則 (SRP)**：高階負責高階業務 (如哪些訊息要加"緊急"標題)，底層專注底層技術 (SMTP 打信、HTTP 呼叫簡訊伺服器)。
- **開閉原則 (OCP)**：要增加 Telegram 通知 (新增 Sender) 或是新增「特價廣告訊息」(新增 Message)，你可以獨立地增加任一維度，且完全無需更動現有程式碼。

### 缺點
- **複雜度提升**：比起直接寫出 `EmailNormalMessage`，Bridge 模式需要你在一開始就清楚劃分什麼是「抽象」、什麼是「實作」，對於簡單的單一維度問題，這無疑是一種過度設計 (Over-engineering)。
