# 封裝（Encapsulation）

這份文件示範何謂封裝，並以兩個範例檔案說明：一個是故意寫得不好的 `BadBankAccount.cs`（封裝性差），另一個是較正確的 `BankAccount.cs`（封裝性好且有適當驗證與例外處理）。

**目標**：讓你能理解封裝的目的、如何實作、以及如何判斷一個類別是否有良好的封裝。

**內容結構**

- 封裝是什麼
- 兩個範例檔案比較
- 常見錯誤與最佳實作
- 如何在專案中應用（包含程式示範）

**注意**：當文件中提到檔案或類別時，請參考工作區內的檔案路徑。

## 什麼是封裝？

封裝是物件導向程式設計的基本原則之一。它把資料（欄位）與操作該資料的方法（行為）包裝在一起，並透過可控制的介面（方法或屬性）公開必要功能，同時隱藏內部實作細節。

封裝的好處：

- **保護不變式**：確保物件永遠保持合法狀態（例如帳戶餘額不能為負）。
- **降低耦合**：使用者不必知道內部如何實作，未來可以改變實作而不影響使用者。
- **更容易測試與除錯**：例外類型與參數驗證能讓錯誤更容易被定位與處理。

## 範例檔案

工作區內有兩個相關檔案：

- `/Users/lijiawei/Desktop/設計模式/DesignPatterns/OopPrinciples/Encapsulation/BadBankAccount.cs`（封裝性差）
- `/Users/lijiawei/Desktop/設計模式/DesignPatterns/OopPrinciples/Encapsulation/BankAccount.cs`（封裝性好）

以下是兩個檔案行為差異的重點說明與程式碼片段解析。

### BadBankAccount（錯誤示範）

主要問題：

- 公開欄位 `balance`，外部程式可以任意讀取與寫入，無法阻止不合法的變更。

範例（摘錄）：

```csharp
public class BadBankAccount
{
	public decimal balance;
}
```

問題解析：

- 當欄位是 `public` 時，任何程式碼都可以做 `bad.balance = -100` 或其他不合法操作。這會讓物件狀態變得不可預期，破壞封裝的目的。

### BankAccount（正確示範）

重點做法：

- 將內部欄位 `balance` 設為 `private`。
- 提供唯讀屬性 `Balance` 或 `GetBalance()` 供外部查詢。
- 提供 `Deposit()` 與 `Withdraw()` 方法，內部包含參數驗證與語意清楚的例外處理。

摘錄重點程式：

```csharp
public class BankAccount
{
	private decimal balance;

	// 提供唯讀屬性讓外部安全讀取餘額（不允許直接寫入）
	public decimal Balance => balance;

	public BankAccount(decimal initialBalance)
	{
		if (initialBalance < 0) throw new ArgumentOutOfRangeException(nameof(initialBalance), "初始餘額不能為負數。");
		balance = initialBalance;
	}

	public void Deposit(decimal amount)
	{
		if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "存款金額必須大於 0。");
		balance += amount;
	}

	public void Withdraw(decimal amount)
	{
		if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "提款金額必須大於 0。");
		if (amount > balance) throw new InvalidOperationException("帳戶餘額不足。");
		balance -= amount;
	}
}
```

說明：

- `ArgumentOutOfRangeException`：用於表示方法參數超出允許範圍，例如存款或提款金額需為正數。
- `InvalidOperationException`：用於表示在目前物件狀態下無法執行請求的操作，例如提款金額超過餘額。

## 常見錯誤與為何要用特定例外

- 不要用 `Exception` 或 `AggregateException` 來表示參數錯誤或業務邏輯錯誤。這些泛用例外會使呼叫端無法精準處理錯誤，也降低 API 可預期性。
- 使用語意明確的例外可以讓上層呼叫者用 `catch (ArgumentOutOfRangeException)` 或 `catch (InvalidOperationException)` 精準處理。
- 若需要更細緻的處理，可以自行定義 `InsufficientFundsException` 繼承 `InvalidOperationException`，讓呼叫端可以針對「餘額不足」做專門邏輯。

## 使用範例（在 Program.cs）

下面範例示範正確使用 `BankAccount`：

```csharp
using System.Collections.Generic;

BankAccount bankAccount = new BankAccount(100);

List<decimal> data = new List<decimal> { 50, 25, 400, 85, 125 };
data.ForEach(bankAccount.Deposit);
Console.WriteLine(bankAccount.Balance);

try
{
	bankAccount.Withdraw(1000);
}
catch (InvalidOperationException ex)
{
	Console.WriteLine("提款失敗：" + ex.Message);
}
```

## 建議修正步驟

1. 保持 `BadBankAccount.cs` 作為示範用（顯示不當做法），但在實際程式中避免使用公開欄位。
2. 將生產程式改用 `BankAccount.cs`，並確保 unit test 覆蓋邊界條件（存入 0、負數、提款超過餘額、剛好提款把餘額變成 0）。
3. 若需要更友善 API，可新增 `TryWithdraw(decimal amount, out decimal remaining)` 回傳布林而非拋例外，供高效或常態失敗（例如競爭性扣款）時使用。

---

如果你要，我可以：

- 把 `Program.cs` 的示範程式碼自動更新成上述範例（會修改檔案）。
- 幫你新增 `InsufficientFundsException` 的範例與如何捕捉。
- 新增單元測試檔案示範邊界測試。

你想要我接著做哪件事？
