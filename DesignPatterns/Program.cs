using DesignPatterns.DesignPatternsCourse.Structural.Bridge;
using DesignPatterns.DesignPatternsCourse.Structural.Flyweight;

// Console.WriteLine("=== Bridge Pattern ===");

// 1. 建立底層實作者 (各種發送管道)
// IMessageSender emailSender = new EmailSender();
// IMessageSender smsSender = new SmsSender();

// 2. 建立高層抽象端並進行【橋接】 (一般訊息 & 緊急訊息)
// Message normalEmailMessage = new NormalMessage(emailSender, "您的訂單已成立，預計三日內出貨。");
// Message urgentSmsMessage = new UrgentMessage(smsSender, "伺服器 CPU 使用率飆高至 95%！");

// 3. 呼叫高層方法。他們會各自依賴其內部注入好的底層實作者完成任務
// normalEmailMessage.Send();
// urgentSmsMessage.Send();

IDocument docForEmployee = new DocumentProxy("Employee");
docForEmployee.Read();
docForEmployee.Write("亂改");

IDocument docForManger = new DocumentProxy("Manager");
docForManger.Read();
docForManger.Write("更新財報為Q4數據");
docForManger.Read();

// === Flyweight 享元模式 ===
Console.WriteLine("\n=== Flyweight 享元模式 ===");

var forest = new Forest();

string[] names    = { "橡樹", "松樹", "楓樹" };
string[] colors   = { "深綠", "翠綠", "橘紅" };
string[] textures = { "粗糙", "細緻", "光滑" };

var rng = new Random(42);
for (int i = 0; i < 12; i++)
{
    int idx = rng.Next(3);
    forest.PlantTree(
        x: rng.Next(100),
        y: rng.Next(100),
        name: names[idx],
        color: colors[idx],
        texture: textures[idx]
    );
}

Console.WriteLine("\n--- 繪製森林 ---");
forest.Draw();

Console.WriteLine($"\n種植了 {forest.TreeCount} 棵樹");
Console.WriteLine($"享元工廠共享物件數量：{TreeFactory.GetCacheSize()} 種 TreeType");
Console.WriteLine($"省記憶體原理：{forest.TreeCount} 棵樹只用了 {TreeFactory.GetCacheSize()} 個 TreeType 物件！");