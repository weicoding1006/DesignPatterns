# 迭代器模式 (Iterator Pattern)

## 這個手寫範例寫得好嗎？

您目前的 `example.cs` 寫得**非常好**，完美表達了 Iterator 模式的核心精神！
它包含了 Iterator 模式中最重要的四個角色：

1. **Iterator (迭代器介面)**: `ITerator<T>`
   定義了存取和走訪元素的操作 (`HasNext()`, `Next()`)。
2. **ConcreteIterator (具體迭代器)**: `LibrayIterator`
   實作如何走訪圖書館 (`_library`) 的邏輯，並記錄目前的走訪進度 (`_currentIndex`)。
3. **Aggregate (集合介面)**: `IBookCollection`
   定義建立迭代器物件的方法 (`CreateIterator()`)。
4. **ConcreteAggregate (具體集合)**: `Library`
   實作集合介面，這裡封裝了一組 `List<Book>`，並能夠回傳對應的迭代器。

**這個設計的最大優點：** 
客戶端 (如 `Program.cs` 的 `while` 迴圈) 不必知道 `Library` 內部是用 `List`、陣列或什麼複雜結構來儲存書本，只要呼叫 `HasNext()` 跟 `Next()` 就能保證拿到所有的書。這完全符合**單一職責原則 (SRP)** (將走訪邏輯從集合中抽離出來) 和**開閉原則 (OCP)** (如果要換各種新的走訪方式，新增 Iterator 類別即可，不用動到 Library)。

---

## C# 中不用手寫的內建方式：`IEnumerable` 與 `yield return`

在實務上開發 C# 應用程式時，**我們幾乎不會自己刻 `ITerator` 介面**。因為 .NET 已經內建了標準的迭代器介面，我們在用的 `foreach` 語法，背後其實就是 Iterator 模式的展現。

*   **`IEnumerable<T>`** 就等同於你的 `IBookCollection` (具備 `GetEnumerator()`，負責產出迭代器)。
*   **`IEnumerator<T>`** 就等同於你的 `ITerator<T>`。

更神的是，C# 提供了 **`yield return`** 語法糖。使用了它，**編譯器會在背後自動幫你產生剛才手寫的那一長串具體迭代器類別 (相當於自動產生了你的 `LibrayIterator`)**。

請參考同資料夾下的 `BuiltInIterator.cs`，這個類別展示了如何極度簡潔地實作一樣的功能：

```csharp
using System.Collections;
using System.Collections.Generic;

// 1. 直接實作內建的 IEnumerable<T>
public class Library : IEnumerable<Book>
{
    private List<Book> _books = new List<Book>();

    public void AddBook(Book book) => _books.Add(book);

    // 2. 實作取得迭代器的方法 (C# 規定叫 GetEnumerator)
    public IEnumerator<Book> GetEnumerator()
    {
        // 3. 神奇的 yield return! 
        // 這裡不需要像手寫版那樣回傳 new LibrayIterator(this)
        // 編譯器看到 yield return，就會為你自動產生具備 MoveNext (同你的 HasNext+Next) 功能的類別
        foreach (var book in _books)
        {
            yield return book;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

### 兩者的對照：怎麼用？

**手寫的用法 (`example.cs`)：**
```csharp
ITerator<Book> iterator = library.CreateIterator();
while(iterator.HasNext())
{
    Book currentBook = iterator.Next();
    Console.WriteLine(currentBook.Title);
}
```

**C# 內建的用法 (`BuiltInIterator.cs`)：**
```csharp
// 背後其實一樣是先呼叫 GetEnumerator() 產生迭代器，然後不斷呼叫 MoveNext() 與 Current
foreach (Book book in library)
{
    Console.WriteLine(book.Title);
}
```

這就是為什麼在 C# 裡我們很少看到有人自己定義 Iterator 介面，因為語言特性已經把它包裝得太好用了！
