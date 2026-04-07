namespace DesignPatterns.DesignPatternsCourse.Structural.Bridge
{
    /// <summary>
    /// Implementor (實作者介面)：
    /// 定義了底層操作的介面，它不一定要與 Abstraction 介面一致。
    /// 事實上，這兩個介面通常是全完不同的。
    /// Implementor 提供基本的操作，而 Abstraction 則在此基礎上定義較高階的商業邏輯。
    /// </summary>
    public interface IMessageSender
    {
        void SendMessage(string title, string body);
    }
}
