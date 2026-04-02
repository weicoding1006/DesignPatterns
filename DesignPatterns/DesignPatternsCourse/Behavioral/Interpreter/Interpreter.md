# 解譯器模式 (Interpreter Pattern)

## 📖 什麼是解譯器模式？

**解譯器模式**是一種**行為型設計模式**。主要目的是為了某一種特定語言或問題，定義出它的文法，並且建立一個**解譯器**來逐一解譯並執行這個語言的句子。
通常會將句子（如一串數學公式或搜尋條件）轉換成一個**抽象語法樹 (Abstract Syntax Tree, AST)**，透過樹狀結構遞迴地去解讀每個節點代表的意義並得出最終結果。

---

## 🏗️ 核心角色

在我們的程式碼中，包含了以下幾種核心物件角色：

1. **Context（上下文）** 
   - 負責保存全域資訊與狀態，供解譯器隨時查詢。
   - 在範例中為 `Context.cs`，它使用 `Dictionary` 儲存了變數名稱與對應的整數值（例如：`a = 10`）。
   
2. **AbstractExpression（抽象表達式）**
   - 定義所有語法節點共通的介面。
   - 在範例中為 `IExpression.cs`，宣告了 `Interpret(Context context)` 方法。

3. **TerminalExpression（終結符表達式）**
   - 語法樹的「葉節點」，不會再向下延伸。通常用來取得單一變數或常數值。
   - 在範例中為 `VariableExpression.cs`，它的作用就是單純向 `Context` 查詢該變數目前對應的值。

4. **NonterminalExpression（非終結符表達式）**
   - 包含其他表達式的節點（運算子），根據文法規則將底下的子表達式進行相應運算。
   - 在範例中為 `AddExpression.cs` (加法) 與 `SubtractExpression.cs` (減法)。它們內部包含 `_left` 與 `_right` 兩個 `IExpression` 屬性，解譯時會先遞迴算出子節點的數值再進行加減。

---

## 💻 程式碼運作解析 

在 `Program.cs` 程式進入點中，我們建構了一棵**抽象語法樹**來模擬 `a + b - c` 的運算：

```csharp
// 1. 初始化 Context 環境並宣告變數
Context context = new Context();
context.Assign("a", 10);
context.Assign("b", 5);
context.Assign("c", 2);

// 2. 準備終端節點 (葉節點)
IExpression a = new VariableExpression("a");
IExpression b = new VariableExpression("b");
IExpression c = new VariableExpression("c");

// 3. 建構抽象語法樹 (AST): (a + b) - c
IExpression expression = new SubtractExpression(
    new AddExpression(a, b), 
    c
);

// 4. 進行解譯與計算
int result = expression.Interpret(context);
Console.WriteLine($"Result of a + b - c  : {result}"); // 預期為 13
```

---

## 🌟 模式的優缺點

### ✅ 優點
* **擴展性極佳 (符合 OCP)**：當你想要加入新的運算子（例如：乘法 `MultiplyExpression` 或是 除法 `DivideExpression`），只要實作 `IExpression` 即可，完全不需要修改既有程式碼。
* **每個文法規則都對應一個類別**：物件職責切分明確。

### ⚠️ 缺點
* **類別爆炸**：如果語言的文法越來越複雜，可能會面臨有成千上萬個 Expression 類別難以維護。
* **效能隱憂**：解譯過程利用了遞迴呼叫，如果語法樹很深（公式很長），可能會有效能瓶頸或是堆疊溢位 (StackOverflow) 的風險。

## 🎯 適用情境分析

不建議隨意在一般商業邏輯開發中使用此模式，但在以下情境非常適合：
1. **需要設計一個簡單的語言或表達式系統**：例如特定的機器手臂操作指令。
2. **規則引擎、過濾器系統**：例如實作電子郵件過濾（`SubjectContainsExpression("VIP")` AND `DateExpression("Today")`）。
3. **簡易自訂的數學公式運算器**。

現代實務上，遇到真正的複雜腳本語言或語法解析需求時，往往會直接交由第三方的剖析工具 (Parser Generator，如 ANTLR) 或是 C# 內建的 `Expression Tree` 進行處理。
