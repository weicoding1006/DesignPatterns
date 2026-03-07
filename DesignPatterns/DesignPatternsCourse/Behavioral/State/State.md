# 狀態模式 (State Pattern)

## 模式介紹
**狀態模式 (State Pattern)** 是一種**行為型設計模式 (Behavioral Design Pattern)**。
它允許一個物件在其內部狀態改變時，改變它的行為。從外部看起來，這個物件彷彿改變了它的類別一樣。

### 解決了什麼問題？
在開發中，我們常常會遇到一個物件擁有多種狀態，且該物件的方法會根據「當前狀態」而有不同的行為表現。
傳統的做法通常是使用大量的 `if-else` 或 `switch-case` 語句來判斷當前狀態。然而，當狀態類型變得越來越多時：
1. **邏輯臃腫**：判斷邏輯會使得程式碼變得難以閱讀和維護。
2. **違反開閉原則 (OCP)**：每次新增一種狀態，都必須回去修改原本龐大的 `switch` 區塊，容易改壞現有邏輯。

狀態模式透過將**每一種狀態封裝成獨立的類別**，把不同狀態下的特定行為分散到這些具體的狀態類別中，從而消除大型的條件判斷語句。這完美符合了**單一職責原則 (SRP)**。

---

## 核心角色與程式碼架構審核

根據現有的程式碼，實作完全符合 GoF 經典狀態模式的標準架構，定義了以下三個核心角色：

### 1. Context (環境類)
對應程式碼：`Document.cs`
- 維護一個對 `State` 物件的參考 (`_state`)，這個參考指向當前狀態。
- 將與狀態相關的操作 (例如 `RequestReview()`, `RequestPublish()`) 委託給該內部狀態物件處理。
- 提供 `SetState(...)` 讓具體狀態類別能夠切換 `Document` 的狀態。

### 2. State (抽象狀態)
對應程式碼：`DocumentState.cs`
- 定義為抽象類別 `DocumentState`。
- 定義了所有具體狀態必須實現的介面 (`Review()` 和 `Publish()`)。
- **(重點)** 持有對 Context (`_document`) 的 protected 參考，讓子類別能用它來觸發狀態變更。

### 3. Concrete State (具體狀態)
對應程式碼：`ConcreteState.cs` (`DraftState`, `ReviewState`, `PublishedState`)
- 繼承 `DocumentState` 並且針對該狀態實作具體的行為。
- 例如：`DraftState` 允許被送審 (`Review`方法)，在送審後主動呼叫 `_document.SetState(new ReviewState())` 將 Context 切換到下一個狀態。

---

## 程式碼執行範例說明

在 `Program.cs` 執行的情境中：

```csharp
// 初始狀態為 草稿 (Draft)
Document doc = new Document(new DraftState());

// 在草稿狀態嘗試發布 -> 被子類別 DraftState 擋下
doc.RequestPublish();
// 輸出：操作失敗：草稿無法直接發佈，必須先經過審核

// 正常流程：草稿送審 -> 進入審核狀態
doc.RequestReview(); 
// 輸出：將草稿送交審核
// 輸出：文件狀態切換至：ReviewState

// 在審核狀態嘗試發布 -> 成功，進入已發佈狀態
doc.RequestPublish(); 
// 輸出：審核通過，文件正式發佈
// 輸出：文件狀態切換至：PublishedState

// 在已發佈狀態嘗試送審 -> 被子類別 PublishedState 擋下
doc.RequestReview();
// 輸出：操作失敗：文件已發佈，不可再送審
```

## 程式碼 Review 總結
目前的 `Document`, `DocumentState`, `DraftState` 等實作**結構非常標準且正確**。
如果您在編譯時看到了 `CS8618` 的警告 (例如 `'_document' 必須包含非 Null 值`)，此為 C# NRT (Nullable Reference Types) 的檢查特性。如果不想看到它，可以在變數宣告加上 `!` 抑制警告，這並不影響狀態模式邏輯的正確性：
```csharp
protected Document _document = null!;
```
