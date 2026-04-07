using System;

namespace DesignPatterns.DesignPatternsCourse.Structural.Bridge
{
    /// <summary>
    /// ConcreteImplementor A
    /// 實作 IMessageSender 的 Email 發送邏輯
    /// </summary>
    public class EmailSender : IMessageSender
    {
        public void SendMessage(string title, string body)
        {
            Console.WriteLine($"[Email 發送] 標題: {title} | 內容: {body}");
        }
    }

    /// <summary>
    /// ConcreteImplementor B
    /// 實作 IMessageSender 的簡訊發送邏輯
    /// </summary>
    public class SmsSender : IMessageSender
    {
        public void SendMessage(string title, string body)
        {
            Console.WriteLine($"[SMS 簡訊發送] 標題: {title} | 內容: {body}");
        }
    }
}
