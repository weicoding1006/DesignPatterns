using DesignPatterns.DesignPatternsCourse.Structural.Bridge;

Console.WriteLine("=== Bridge Pattern ===");

// 1. 建立底層實作者 (各種發送管道)
IMessageSender emailSender = new EmailSender();
IMessageSender smsSender = new SmsSender();

// 2. 建立高層抽象端並進行【橋接】 (一般訊息 & 緊急訊息)
Message normalEmailMessage = new NormalMessage(emailSender, "您的訂單已成立，預計三日內出貨。");
Message urgentSmsMessage = new UrgentMessage(smsSender, "伺服器 CPU 使用率飆高至 95%！");

// 3. 呼叫高層方法。他們會各自依賴其內部注入好的底層實作者完成任務
normalEmailMessage.Send();
urgentSmsMessage.Send();