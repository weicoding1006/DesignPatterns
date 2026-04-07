namespace DesignPatterns.DesignPatternsCourse.Structural.Bridge
{
    /// <summary>
    /// Abstraction (抽象層)：
    /// 定義高層的控制邏輯，它依賴於對 Implementor (IMessageSender) 的橋接。
    /// 可以看到它包含一個對 IMessageSender 介面的參考。
    /// </summary>
    public abstract class Message
    {
        // 橋接 (Bridge) 核心：保留一個實作者的參照
        protected IMessageSender _messageSender;

        protected Message(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        // 抽象的高層行為
        public abstract void Send();
    }
}
