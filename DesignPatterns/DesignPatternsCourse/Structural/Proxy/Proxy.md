# 代理模式 (Proxy Pattern)

## 📌 模式簡介

**代理模式 (Proxy Pattern)** 屬於**結構型設計模式**。它可以在不改變原有程式碼的情況下，為其他物件提供一個「代理人」或「佔位符」，以控制對這個物件的存取。

當我們需要對一個物件的存取進行額外控制（例如：權限檢查、延遲載入、添加快取、網路連線等），我們不直接與真實物件互動，而是透過代理物件 (Proxy)。代理物件擁有和真實物件完全相同的介面，因此在客戶端眼中，它們是完全一樣的。

## 🎯 這次程式碼的主題：保護代理與虛擬代理 (Protection & Virtual Proxy)

從以上的程式碼中，我們實作了一個混合了**保護代理（權限控制）**與**虛擬代理（延遲載入）**的實際範例。

### 1. 共通介面 (`IDocument`)
無論是代理物件還是真實物件，都必須實作此介面，這樣客戶端才能用一致的方式來呼叫它們。

```csharp
public interface IDocument
{
    void Read();
    void Write(string content);
}
```

### 2. 真實物件 (`SensitiveDocument`)
這是真正負責儲存與操作機密資料的類別。這個類別單純負責業務邏輯，不處理任何權限判斷。

```csharp
public class SensitiveDocument : IDocument
{
    private string _content = "這是公司機密財報";
    
    public void Read()
    {
        Console.WriteLine($"[閱讀文件] {_content}");
    }

    public void Write(string content)
    {
        _content = content;
        Console.WriteLine($"[修改文件] 內容已更新");
    }
}
```

### 3. 代理物件 (`DocumentProxy`)
代理物件內部會持有一個 `SensitiveDocument`（真實物件）的參考，並在呼叫前後「加料」：
*   **虛擬代理 (延遲載入)**：在 `Read()` 或 `Write()` 中，只有當 `_realDocument` 為 `null` 時才進行 `new SensitiveDocument()` 的初始化。如果那是一個消耗極大資源的物件，這能大幅節省效能與記憶體。
*   **保護代理 (權限檢查)**：在 `Write()` 中，會先確認使用者的角色 `_userRole` 是否為 `"Manager"`，只有經理可以修改，否則直接拒絕，不呼叫真實物件。

```csharp
public class DocumentProxy : IDocument
{
    private SensitiveDocument _realDocument;
    private string _userRole;
    
    public DocumentProxy(string userRole)
    {
        _userRole = userRole;
    }
    
    public void Read()
    {
        // 延遲載入 (Lazy Initialization)
        if(_realDocument == null)
        {
            _realDocument = new SensitiveDocument();
        }
        _realDocument.Read();
    }

    public void Write(string content)
    {
        // 權限控制
        if(_userRole != "Manager")
        {
            Console.WriteLine($"[存取被拒]角色 {_userRole}沒有修改機密文件的權限");
            return;
        }

        if(_realDocument == null)
        {
            _realDocument = new SensitiveDocument();
        }
        _realDocument.Write(content);
    }
}
```

### 4. 客戶端呼叫 (`Program.cs`)
在客戶端呼叫時，我們總是與 `DocumentProxy` 互動，代理物件完美的負責了把關的職責。

```csharp
// 一般員工的情境
IDocument docForEmployee = new DocumentProxy("Employee");
docForEmployee.Read();          // 允許 (會印出機密內容)
docForEmployee.Write("亂改");   // [存取被拒]角色 Employee沒有修改機密文件的權限

// 經理的情境
IDocument docForManager = new DocumentProxy("Manager");
docForManager.Read();            // 允許
docForManager.Write("更新財報為Q4數據"); // 允許 (修改成功)
docForManager.Read();            // 印出更新後的內容
```

## 🛠️ 常見應用場景
1.  **延遲初始化 (虛擬代理)**：如果建立一個物件很消耗資源，可以在真正需要使用它時，才讓代理物件建立它。
2.  **存取控制 (保護代理)**：攔截請求，拒絕無權限的呼叫，像我們上面做的權限檢查。
3.  **快取代理 (Cache Proxy)**：把非常耗時的運算結果或 API 請求存在代理類別中，遇到相同的請求就直接吐回，不用再去呼叫真實物件。
4.  **遠端代理 (Remote Proxy)**：為在不同位址空間 (例如不同伺服器、API) 的物件提供一個本地端的代理人。

## 💡 與其他模式的差別
*   **代理模式 (Proxy)**：重於「控制存取」。
*   **裝飾者模式 (Decorator)**：重於「動態添加功能」。(兩者結構很像，但目標不同)
*   **轉接器模式 (Adapter)**：重於「介面轉換」。
