# 中介者模式 (Mediator Pattern)

## 什麼是中介者模式？
中介者模式是一種**行為型設計模式**。它用來降低多個物件或類別之間的通訊複雜度。
透過引入一個「中介者 (Mediator)」物件，原本互相依賴、直接通訊的物件們（同事物件 Colleagues）不再直接對話，而是統一將訊息交給中介者，由中介者負責路由與轉發。

這樣可以將系統中混亂的 **網狀依賴 (M:N)** 轉化為結構清晰的 **星狀依賴 (1:N)**。

生活中的最佳例子就是「**機場塔台**」或「**聊天室伺服器**」。使用者之間不互相儲存對方的位址，而是將訊息傳給聊天室，由聊天室決定要把訊息轉發給誰（廣播或私訊）。

---

## 角色與程式碼對應

在目前的專案目錄中，我們實作了一個支援「群組廣播」與「單一私訊」的聊天室：

### 1. 中介者介面 (Mediator)
**📝 `IChatRoomMediator.cs`**
* 定義了中介者必須提供的服務：
    * `RegisterUser(MediatorUser user)`：讓使用者加入聊天室。
    * `SendMessage(string message, MediatorUser sender)`：群組廣播訊息。
    * `SendPrivateMessage(string message, MediatorUser sender, string receiverName)`：發送私人訊息給特定對象。

### 2. 具體中介者 (Concrete Mediator)
**📝 `ChatRoom.cs`**
* 實作聊天室的核心邏輯，內部維護了一個使用者名單 (`List<MediatorUser> _users`)。
* **廣播邏輯 (`SendMessage`)**：收到訊息時，遍歷使用者清單，除了發送者之外，呼叫每個人的 `Receive` 方法。
* **私訊邏輯 (`SendPrivateMessage`)**：收到私訊請求時，根據 `receiverName` 在名單中尋找目標使用者。如果找到，就單獨觸發他的 `Receive` 方法，並在訊息加上 `[私訊]` 前綴，實現了點對點的私訊溝通。

### 3. 同事類別 (Colleague)
**📝 `MediatorUser.cs`**
* 代表聊天室中的使用者。
* 每個使用者只保存對 `IChatRoomMediator` (中介者) 的參考 `_chatRoom`，而**不需要認識其他任何使用者**。
* 提供 `Send` (群組廣播) 與 `SendPrivate` (單獨私訊) 方法。當要發送訊息時，只需呼叫 `_chatRoom` 對應的方法即可，發送端不需要自己找出所有的接收者。

---

## 實際執行流程與範例

根據我們在 `Program.cs` 剛剛測試的執行過程，廣播與私訊的結果如下：

```csharp
// 1. 建立中介者 (聊天室)
var charRoom = new ChatRoom();

// 2. 建立使用者 (不會互相擁有對方的記憶體參考)
var alice = new MediatorUser("Alice");
var bob = new MediatorUser("Bob");
var jeff = new MediatorUser("Jeff");

// 3. 使用者向中介者註冊
charRoom.RegisterUser(alice);
charRoom.RegisterUser(bob);
charRoom.RegisterUser(jeff);

// 4. Alice 發送廣播訊息給大家
alice.Send("哈囉大家好");
/* 輸出：
Alice 發送的訊息 : 哈囉大家好
Bob收到來自Alice的訊息:哈囉大家好
Jeff收到來自Alice的訊息:哈囉大家好
*/

// 5. Jeff 發送廣播訊息給大家
jeff.Send("嗨 Alice");
/* 輸出：
Jeff 發送的訊息 : 嗨 Alice
Alice收到來自Jeff的訊息:嗨 Alice
Bob收到來自Jeff的訊息:嗨 Alice
*/

// 6. Jeff 傳送私訊給 Alice (此時 Bob 就不會收到)
jeff.SendPrivate("測試", "Alice");
/* 輸出：
Alice收到來自Jeff的訊息:[私訊] 測試
*/
```

---

## 為什麼要使用中介者模式？

1. **降低耦合度 (Decoupling)**：
   `MediatorUser` 不依賴其他的 `MediatorUser`，可以輕易的新增、刪除或修改某個使用者，完全不用擔心會破壞其他人之間的連繫。所有的連繫邏輯都封裝在 `ChatRoom` 裡面。
   
2. **集中控制互動邏輯**：
   如果日後想要加入「過濾髒話」、「限制某人發言」或是「只允許特定身分廣播」的功能，只需要在 `ChatRoom` (中介者) 的類別中修改程式碼即可，不需要改動到每一個使用者的類別。
