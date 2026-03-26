# 觀察者模式 (Observer Pattern)

## 什麼是觀察者模式？
觀察者模式是一種**行為型設計模式**。它定義了物件之間的一對多依賴關係：當一個物件（被觀察者 / Subject）的狀態發生改變時，所有依賴於它的物件（觀察者 / Observers）都會自動得到通知並進行更新。

生活中的最佳例子就是「YouTube 頻道訂閱機制」。當頻道發布新影片時，所有訂閱該頻道的粉絲都會收到推播通知，不需要用戶手動去檢查。

---

## 角色與程式碼對應

在目前的專案目錄中，我們使用了以下角色來實作觀察者模式：

### 1. 觀察者介面 (Observer)
**📝 `IObserver.cs`**
* 定義了觀察者接收通知的標準。
* 包含一個 `Update(string videoTitle)` 方法，讓推播者在事件發生時可以呼叫並傳遞資料 (影片標題)。

### 2. 主題/被觀察者介面 (Subject)
**📝 `ISubject.cs`**
* 宣告了管理觀察者的標準行為：
    * `Attach(IObserver observer)`：註冊/訂閱。
    * `Detach(IObserver observer)`：移除/取消訂閱。
    * `Notify()`：觸發並通知所有已訂閱的觀察者。

### 3. 具體主題 (Concrete Subject)
**📝 `YouTubeChannel.cs`**
* 實作了 `ISubject` 介面，扮演具體的被觀察者角色。
* 內部維護了一個訂閱者名單 (`List<IObserver> _observers`)。
* 當呼叫 `UploadVideo("影片名稱")` 發布新影片時，它會更新自己的狀態 (`_latestVideoTitle`)，然後立刻呼叫 `Notify()`，逐一觸發名單內每個觀察者的 `Update` 方法。

### 4. 具體觀察者 (Concrete Observer)
**📝 `Subscriber.cs`**
* 實作了 `IObserver` 介面，扮演具體的粉絲/訂閱者。
* 每個粉絲擁有自己的名稱 (`_name`)。
* 在實作的 `Update` 方法中定製了自己的反應邏輯（例如在終端機印出收到影片通知的訊息）。

---

## 實際執行流程與範例

根據 `Program.cs` 的呼叫方式，整個模式的運轉邏輯如下：

```csharp
// 1. 建立一個被觀察者 (YouTube 頻道)
var channel = new YouTubeChannel();

// 2. 建立具體的觀察者 (兩位粉絲)
var sub1 = new Subscriber("Jeff");
var sub2 = new Subscriber("Hani");

// 3. 粉絲向頻道「訂閱」 (加入觀察者清單)
channel.Attach(sub1);
channel.Attach(sub2);

// 4. 頻道狀態改變 (發布新影片)，會自動推播給清單內的所有人
channel.UploadVideo("C#教學");
// 終端機輸出：
// 頻道發布了新影片:C#教學
// Jeff收到通知了!趕快去看:C#教學
// Hani收到通知了!趕快去看:C#教學

// 5. Jeff 覺得不想看了，選擇「取消訂閱」
channel.Detach(sub1);

// 6. 頻道再次發布影片，這時只剩下尚未退訂的 Hani 收到通知
channel.UploadVideo("工程師必看");
// 終端機輸出：
// 頻道發布了新影片:工程師必看
// Hani收到通知了!趕快去看:工程師必看
```

---

## 為什麼要使用觀察者模式？

1. **極度解耦 (Decoupling)**：
   `YouTubeChannel` 完全不需要知道 `Subscriber` 的內部實作細節，它只要確保所有訂閱的人都有實作 `IObserver` 介面就好。這降低了類別之間的相依性。
   
2. **符合開閉原則 (Open/Closed Principle)**：
   未來如果想要增加不同的觀察者！例如，新增一個 `EmailNotificationSystem` 或者 `DiscordBot`，只要讓它們實作 `IObserver` 介面並加入 `Attach` 中即可，**完全不需修改** `YouTubeChannel` 原本的程式碼！
