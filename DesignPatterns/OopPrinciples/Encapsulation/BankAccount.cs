
namespace DesignPatterns.OopPrinciples.Encapsulation;

public class BankAccount
{
    private decimal balance;

    // 提供唯讀屬性讓外部安全讀取餘額（不允許直接寫入）
    public decimal Balance => balance;

    // 建構子：使用初始餘額建立帳戶
    // 如果輸入的 initialBalance 小於 0，會拋出 ArgumentOutOfRangeException
    public BankAccount(decimal initialBalance)
    {
        if (initialBalance < 0)
        {
            // ArgumentOutOfRangeException：表示傳入的參數超出可接受的範圍
            throw new ArgumentOutOfRangeException(nameof(initialBalance), "初始餘額不能為負數。");
        }

        balance = initialBalance;
    }


    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            // ArgumentOutOfRangeException：用於表示方法參數不在有效範圍
            // 這比直接丟 Exception 更清楚，呼叫端可以根據此例外知道是參數問題
            throw new ArgumentOutOfRangeException(nameof(amount), "存款金額必須大於 0。");
        }

        balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            // 同上，參數不在合理範圍
            throw new ArgumentOutOfRangeException(nameof(amount), "提款金額必須大於 0。");
        }

        if (amount > balance)
        {
            // InvalidOperationException：表示在目前物件狀態下無法執行這個操作
            // 這比一般 Exception 更語意化，呼叫端可以判斷這是業務邏輯（狀態）問題
            throw new InvalidOperationException("帳戶餘額不足。");
        }

        balance -= amount;
    }
}