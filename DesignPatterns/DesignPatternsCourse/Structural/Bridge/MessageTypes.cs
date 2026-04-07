namespace DesignPatterns.DesignPatternsCourse.Structural.Bridge
{
    /// <summary>
    /// RefinedAbstraction (擴充抽象層)：普通訊息
    /// </summary>
    public class NormalMessage : Message
    {
        private string _content;

        public NormalMessage(IMessageSender messageSender, string content) 
            : base(messageSender)
        {
            _content = content;
        }

        public override void Send()
        {
            // 將高層級的「普通訊息」轉換成底層的方法呼叫
            _messageSender.SendMessage("【一般通知】", _content);
        }
    }

    /// <summary>
    /// RefinedAbstraction (擴充抽象層)：緊急訊息
    /// 可以加上不同的處理邏輯，例如加上警告標語或重新發送機制。
    /// </summary>
    public class UrgentMessage : Message
    {
        private string _content;

        public UrgentMessage(IMessageSender messageSender, string content) 
            : base(messageSender)
        {
            _content = content;
        }

        public override void Send()
        {
            // 加上高層級才有的緊急邏輯
            string urgentContent = $"!!! 緊急情況 !!! {_content}";
            _messageSender.SendMessage("【緊急通知】", urgentContent);
        }
    }
}
