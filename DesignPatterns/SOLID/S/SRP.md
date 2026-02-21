# SOLID 原則 - S：單一職責原則 (Single Responsibility Principle, SRP)

**單一職責原則**的核心概念是：**「一個類別應該只有一個改變的理由」**(A class should have one, and only one, reason to change)。換句話說，一個類別只應該負責一項專門的職責或功能。

在這三份 C# 檔案中，我們可以清楚看到系統的職責是如何被正確拆分與封裝的，這正是單一職責原則的良好實踐：

### 1. `User.cs` - 資料模型 (Data Model)
```csharp
public class User
{
    public string UserName { get; set; }
    public string Email { get; set; }
}
```
* **核心職責**：專門用來定義與儲存使用者的基本資料屬性。
* **改變的理由**：只有當「使用者的資料結構」發生變化（例如：需要新增電話號碼欄位、生日等）時，我們才會去修改這個類別。

---

### 2. `SOLIDEmailSender.cs` - 基礎通訊設施 (Infrastructure / Notification)
```csharp
public class SOLIDEmailSender
{
    public void SendEmail(string email, string message)
    {
        Console.WriteLine($"寄送至{email} : {message}");
    }
}
```
* **核心職責**：專門負責「寄送 Email 通知」的行為實作。
* **改變的理由**：只有當「寄件方式或底層邏輯」發生變化（例如：改用第三方 Email API 像是 SendGrid 或是修改 SMTP 設定）時，我們才會修改這個類別，它完全不關心是什麼業務需要寄信。

---

### 3. `UserService.cs` - 商業邏輯 (Business Logic)
```csharp
public class UserService
{
    public void Register(User user)
    {
        SOLIDEmailSender emailSender = new SOLIDEmailSender();
        emailSender.SendEmail(user.Email, "註冊");
    }
}
```
* **核心職責**：專門負責處理「與使用者相關的商業邏輯」流程（在這裡對應的是註冊流程）。
* **改變的理由**：只有當「註冊的商業流程」發生變化（例如：註冊前需要檢查帳號是否重複、註冊後要寫入資料庫）時，我們才會修改這個類別。
* **備註**：`UserService` 雖然需要寄信，但它沒有親自實作寄信的工作，而是把這個任務「委派」(Delegate) 給了專門的 `SOLIDEmailSender` 來做。

---

### 💡 如果不遵守單一職責原則（反面案例）

假設我們將所有職責都放進同一個類別中，程式碼可能會長這樣：

```csharp
// 違反 SRP 的設計：萬能類別 (God Class)
public class UserService 
{
    public void Register(string userName, string email) 
    {
        // 1. 商業邏輯：檢查處理註冊...
        
        // 2. 也是這個類別親自實作寄信邏輯
        Console.WriteLine($"建立 SMTP 連線...");
        Console.WriteLine($"寄送至{email} : 註冊成功");
    }
}
```

如此一來，這個 `UserService` 就同時擁有了**兩個**改變的理由：
1. 註冊流程改變時，必須改它。
2. 寄信的連線方式改變時，也必須改它。

一旦類別承擔了過多責任（俗稱的「義大利麵條程式碼」或是「萬能類別」），日後在修改寄信功能時，很可能不小心改壞了註冊邏輯，造成系統難以維護以及難以撰寫單元測試。

### 結論
這三份程式碼透過將「資料 (User)」、「商業邏輯 (UserService)」與「基礎設施 (SOLIDEmailSender)」各自分離，完美地落實了**單一職責原則 (SRP)**。每個類別各司其職，不僅提升了程式設計的內聚力 (Cohesion)，也讓未來針對單一部分的維護與擴展變得更安全與容易。
