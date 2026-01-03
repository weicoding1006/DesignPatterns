# 抽象化 (Abstraction)

抽象化（Abstraction）是物件導向程式設計（OOP）的核心原則之一。它的重點在於**「隱藏複雜的實現細節，只呈現必要的功能介面」**。

## 核心概念

1.  **降低複雜度**：使用者不需要知道背後繁瑣的處理過程，只需要知道如何呼叫簡單的方法即可。
2.  **關注於「做什麼」而非「怎麼做」**：對外暴露的是「功能」（什麼），隱藏的是「邏輯」（如何）。
3.  **封裝的延伸**：抽象化通常透過封裝（Encapsulation）來實現，將內部的細節封裝起來，僅對外顯示一個精簡的抽象層。

---

## 範例解析：`EmailService.cs`

在 `EmailService` 類別中，抽象化被完美地運用了：

```csharp
public class EmailService
{
    // 抽象化的介面：外部呼叫者只需要看這一個「開關」
    public void SendEmail()
    {
        // 內部的複雜流程被隱藏在這個簡單的方法下
        Connect();
        Authenticate();
        Disconnect();
    }

    // 這些是實現細節，對使用者來說被「抽象化」掉了
    private void Connect() => Console.WriteLine("Connecting to server...");
    private void Authenticate() => Console.WriteLine("Authenticating...");
    private void Disconnect() => Console.WriteLine("Disconnecting...");
}
```

### 為什麼這是抽象化？

*   **簡化界面**：寄信的過程可能涉及協議處理、驗證機制、連線維護等數百行程式碼。但透過抽象化，外部使用者只需要呼叫一個簡單的 `SendEmail()`。
*   **隔離變化**：如果明天我們要將郵件伺服器從 SMTP 改成 API，我們只需要修改 `EmailService` 內部的實現，對外部開發者來說，呼叫 `SendEmail()` 的方式完全不變。

---

## 抽象化 vs. 封裝 (Abstraction vs. Encapsulation)

這兩個概念經常被混淆，但它們的視角不同：

*   **封裝 (Encapsulation)**：強調的是**資料的保護與資料行為的打包**（使用 `private`、`public` 來隱藏內部屬性）。
*   **抽象化 (Abstraction)**：強調的是**隱藏實現細節以減少複雜度**（讓使用者看見更高級別的視角）。

> [!TIP]
> 簡單來說：封裝是**「手段」**，抽象化是**「目的」**。

## 抽象化的好處

*   **更容易閱讀與理解**：程式碼變得直覺，像在讀人類語言。
*   **減少錯誤**：使用者不必處理底層邏輯，減少了因為操作不當（例如忘了斷開連線）導致的錯誤。
*   **良好的維護性**：底層邏輯隨便改，只要介面（方法名與參數）不變，系統就不會崩潰。
