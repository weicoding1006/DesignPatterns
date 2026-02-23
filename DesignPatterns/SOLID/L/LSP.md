# 里氏替換原則 (Liskov Substitution Principle - LSP)

**定義：**
子類別必須能夠替換掉它們的基底類別（父類別），而不會影響程式的正確性。換句話說，如果程式中使用的是基底類別，那麼把這個基底類別換成它的子類別，程式的行為應該保持正常，不該出現錯誤或異常。

---

## 為什麼 `BadExample.cs` 違反了 LSP？

在 `BadExample.cs` 中，我們定義了一個基底類別 `BadBird`，它有一個 `Fly()` 方法：

```csharp
public class BadBird
{
    public virtual void Fly()
    {
        Console.WriteLine("這隻鳥在天空飛翔!");
    }
}
```

接著我們讓 `BadPenguin` (企鵝) 繼承 `BadBird`。但是企鵝不會飛，所以我們在子類別中覆寫 (Override) 了 `Fly()` 方法並拋出例外 (Exception)：

```csharp
public class BadPenguin : BadBird
{
    public override void Fly()
    {
        throw new NotImplementedException("企鵝不會飛");
    }
}
```

**違反原因：**
如果有一個系統原本預期接收一個 `BadBird` 物件，並呼叫它的 `Fly()` 方法：
```csharp
public void MakeBirdFly(BadBird bird)
{
    bird.Fly(); // 如果傳入的是 BadPenguin，這裡就會直接拋出例外導致系統崩潰！
}
```
在這裡，`BadPenguin` (子類別) **無法** 無縫替換 `BadBird` (父類別)，因為替換後會導致原本預期能飛的系統操作失敗。這就違反了里氏替換原則。

---

## `Example.cs` 如何符合 LSP？

為了解決這個問題，我們需要重新設計類別的繼承關係與行為，確保繼承關係的合理性。在 `Example.cs` 中，我們將「鳥」和「會飛」的行為分開：

1. **定義共用的基底類別：** `Bird` 類別只保留所有鳥類真正共通的行為（例如 `Eat` 吃東西）。
```csharp
public class Bird
{
    public virtual void Eat()
    {
        Console.WriteLine("這隻鳥在吃東西");
    }    
}
```

2. **將特定的能力抽離成介面：** 針對不是所有鳥類都會的技能（如飛行），定義獨立的介面 `IFlyable`。
```csharp
public interface IFlyable
{
    void Fly();
}
```

3. **子類別各自實作符合自己的行為：**
   - 麻雀 `Sparrow` 是鳥且會飛，所以繼承 `Bird` 並實作 `IFlyable`。
   - 企鵝 `Penguin` 是鳥但不會飛，所以只單純繼承 `Bird`。

```csharp
public class Sparrow : Bird, IFlyable
{
    public void Fly()
    {
        Console.WriteLine("麻雀在天空飛");
    }
}

public class Penguin : Bird
{
    // 企鵝只需要繼承 Bird (擁有 Eat 的能力)，因為牠不會飛，所以不實作 IFlyable
}
```

**符合原因：**
- 如果系統某處需要一隻鳥 (`Bird`) 並且讓牠吃東西，不管傳入的是 `Sparrow` 還是 `Penguin`，呼叫 `Eat()` 都能正常運作，兩者皆完美扮演 `Bird` 的角色並進行替換。
- 如果系統某處需要一個「能夠飛行的物件」(`IFlyable`) 並呼叫 `Fly()`，在呼叫端的方法簽章只能接收有實作 `IFlyable` 的物件（如傳入 `Sparrow`），編譯器層級就把企鵝阻擋在外了。這從根本上避免了把企鵝傳入而導致程式例外崩潰的問題。

藉由將不共通的行為（飛行）移出基底類別，改用介面來規範額外能力，我們成功設計出了符合 **LSP 里氏替換原則** 的程式結構。
