using DesignPatterns.DesignPatternsCourse.Creational;
using DesignPatterns.DesignPatternsCourse.Creational.Singleton;
using DesignPatterns.DesignPatternsCourse.Creational.FactoryMethod;
using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory;
using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Abstracts;
using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Windows;
using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Mac;
using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Linux;
using DesignPatterns.DesignPatternsCourse.Structural.Bridge;
using DesignPatterns.DesignPatternsCourse.Structural.Flyweight;
using DesignPatterns.Structural.Decorator;
using DesignPatterns.Structural.Facade;

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

// IDocument docForEmployee = new DocumentProxy("Employee");
// docForEmployee.Read();
// docForEmployee.Write("亂改");

// IDocument docForManger = new DocumentProxy("Manager");
// docForManger.Read();
// docForManger.Write("更新財報為Q4數據");
// docForManger.Read();

// === Flyweight 享元模式 ===
// Console.WriteLine("\n=== Flyweight 享元模式 ===");

// var forest = new Forest();

// string[] names    = { "橡樹", "松樹", "楓樹" };
// string[] colors   = { "深綠", "翠綠", "橘紅" };
// string[] textures = { "粗糙", "細緻", "光滑" };

// var rng = new Random(42);
// for (int i = 0; i < 12; i++)
// {
//     int idx = rng.Next(3);
//     forest.PlantTree(
//         x: rng.Next(100),
//         y: rng.Next(100),
//         name: names[idx],
//         color: colors[idx],
//         texture: textures[idx]
//     );
// }

// Console.WriteLine("\n--- 繪製森林 ---");
// forest.Draw();

// Console.WriteLine($"\n種植了 {forest.TreeCount} 棵樹");
// Console.WriteLine($"享元工廠共享物件數量：{TreeFactory.GetCacheSize()} 種 TreeType");
// Console.WriteLine($"省記憶體原理：{forest.TreeCount} 棵樹只用了 {TreeFactory.GetCacheSize()} 個 TreeType 物件！");

// === Facade 模式（外觀模式 / 門面模式 ===
// var projector = new Projector();
// var sound = new SoundSystem();
// var lights = new Lights();

// var homeTheaterFacade = new HomeTheaterFacade(projector,sound,lights);
// homeTheaterFacade.WatchMovie();
// homeTheaterFacade.EndMovie();



// Decorator 模式（裝飾者模式)
// 1. 點一杯基本紅茶
// IBeverage myDrink = new BlackTea();
// Console.WriteLine($"{myDrink.GetDescription()} = ${myDrink.GetCost()}");
//2. 加牛奶
// myDrink = new Milk(myDrink);
// Console.WriteLine($"{myDrink.GetDescription()} = ${myDrink.GetCost()}");
//3. 再加珍珠
// myDrink = new Boba(myDrink);
// Console.WriteLine($"{myDrink.GetDescription()} = ${myDrink.GetCost()}");

// 4. 瘋狂客製化：雙份珍珠綠茶
// IBeverage crazyDrink = new Boba(new Boba(new GreenTea()));
// Console.WriteLine($"{crazyDrink.GetDescription()} = ${crazyDrink.GetCost()}");

// // Singleton
// var config1 = AppConfig.Instance;
// var config2 = AppConfig.Instance;
// Console.WriteLine($"App : {config1.ApplicationName}");
// Console.WriteLine($"Version: {config1.Version}");
// config1.UpdateVersion("1.1.0");
// Console.WriteLine($"App : {config2.ApplicationName}");
// Console.WriteLine($"Version: {config2.Version}");

// // === Factory Method 工廠方法模式 ===
// LogisticsCreator logistics = new RoadLogistics();
// logistics.PlanDelivery("電腦零件 x 100箱");
// logistics = new SeaLogistics();
// logistics.PlanDelivery("重型機械 x 5台");

// === Abstract Factory 抽象工廠模式 ===
Console.WriteLine("=== Abstract Factory Pattern ===");

// 情境 1：偵測到目前執行環境為 Windows，注入 Windows 工廠
// UIApplication 只依賴 IUIFactory，完全不感知是 Windows 還是 Mac
Console.WriteLine("\n[ 平台：Windows ]");
IUIFactory factory = new WindowsUIFactory();
var app = new UIApplication(factory);
app.RenderUI();
app.SimulateInteraction();

// 情境 2：切換到 Mac 環境，只要換掉工廠，UI 風格自動一致
// UIApplication 的程式碼一行都不需要改！
Console.WriteLine("\n[ 平台：Mac ]");
factory = new MacUIFactory();
app = new UIApplication(factory);
app.RenderUI();
app.SimulateInteraction();

// 情境 3：擴充 Linux 環境，同樣不需更改 Client 程式碼
Console.WriteLine("\n[ 平台：Linux ]");
factory = new LinuxUIFactory();
app = new UIApplication(factory);
app.RenderUI();
app.SimulateInteraction();
