# 備忘錄模式 (Memento Pattern)

## 模式介紹
這個程式碼實作的是**備忘錄模式 (Memento Pattern)**，它屬於**行為型模式 (Behavioral Pattern)**。

備忘錄模式的主要目的是在**不破壞封裝性**的前提下，捕獲並儲存一個物件的內部狀態，以便在未來可以將該物件恢復到這個狀態。這也就是我們常見的程式「復原/上一步 (Undo)」功能的核心實作機制。

## 角色對應與程式碼分析
在這個範例中，完美對應了該模式的三個核心角色：

1. **Originator (發起人) - `Editor.cs`**
   - 包含需要被備份與恢復狀態的屬性 (`Title`, `Content`)。
   - 提供建立備忘錄的方法：`CreateState()` 會抓取當前狀態並封裝到 `EditorState` 中。
   - 提供恢復狀態的方法：`Restore(EditorState state)` 可將自身狀態蓋回為備忘錄裡記憶的狀態。

2. **Memento (備忘錄) - `EditorState.cs`**
   - 負責儲存發起人（`Editor`）的內部快照狀態（`_title`、`_content` 和建立時間 `_stateCreatedAt`）。
   - **特性**：備忘錄通常被設計為**不可變的 (Immutable)**，為了防止外部（如 `History`）竄改歷史紀錄。你可以看到所有的狀態都只透過建構子傳入，並且只提供 `Get...()` 唯讀方法，沒有 `Set`。

3. **Caretaker (管理者/負責人) - `History.cs`**
   - 負責保存與管理一系列的備忘錄快照（使用 `List<EditorState>` 儲存）。
   - 決定「何時」該備份（調用 `Backup()`）以及「何時」該復原（調用 `Undo()`）。
   - 注意：它**完全不關心/不修改**備忘錄裡面的具體內容，它只負責將之前存起來的 `EditorState` 物件再交還給 `Editor` 進行復原。

## 使用方法 (C# 範例)
你可以透過以下的模擬情境來測試與使用這些類別：

```csharp
class Program
{
    static void Main(string[] args)
    {
        // 1. 初始化 Originator (編輯器) 和 Caretaker (歷史紀錄管理者)
        Editor editor = new Editor();
        History history = new History(editor);

        // 2. 進行一些編輯與備份
        editor.Title = "第一版標題";
        editor.Content = "這是第一版的內容...";
        history.Backup(); // 存檔 (建立快照1)

        editor.Title = "第二版標題";
        editor.Content = "修改了第二版的內容，新增了一些文字!";
        history.Backup(); // 存檔 (建立快照2)

        editor.Title = "第三版標題 (修改中)";
        editor.Content = "這是在第三版的編輯，但我還沒備份...";

        Console.WriteLine($"目前編輯器狀態: {editor.Title} | {editor.Content}\n");

        Console.WriteLine("歷史紀錄清單：");
        history.ShowHistory(); // 顯示有兩筆紀錄
        Console.WriteLine("\n------------------\n");

        // 3. 執行復原 (Undo)
        history.Undo(); 
        Console.WriteLine($"第一次 Undo (退回上一版): {editor.Title} | {editor.Content}");
        
        history.Undo();
        Console.WriteLine($"第二次 Undo (退回最初版): {editor.Title} | {editor.Content}");
    }
}
```

## 備忘錄模式的優缺點

**優點：**
- **保持物件的封裝性**：發起人 (`Editor`) 將自己的私有/內部狀態存入備忘錄 (`EditorState`)，負責管理的物件 (`History`) 只能保管它，無法也無權修改被存起來的狀態細節。
- **簡化發起人的職責**：將「保存歷史紀錄」與「狀態清單管理」的責任抽離並交給 `History` 來處理，讓 `Editor` 類別可以專心處理自己核心的編輯業務邏輯（符合單一職責原則 SRP）。

**缺點：**
- **記憶體資源消耗大**：如果需要保存的狀態資料非常巨大，或者頻繁備份導致歷史紀錄串列拉得太長，就會消耗大量的系統記憶體（這個範例使用了 `List<EditorState>` 不斷增加，若無上限管制會有記憶體問題）。
