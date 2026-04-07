using System.Numerics;
using DesignPatterns.Behavioral.Chain_of_Responsibility;
using DesignPatterns.Behavioral.Interpreter;
using DesignPatterns.Behavioral.Visitor;
using DesignPatterns.DesignPatternsCourse.Structural.Adapter;
using DesignPatterns.OopPrinciples.Encapsulation;
using DesignPatterns.Structural.Composite;

// BankAccount bankAccount = new(100);

// List<decimal> data = [50,25,400,85,125];
// data.ForEach(bankAccount.Deposit);
// Console.WriteLine(bankAccount.Balance);


// EmailService emailService = new();
// emailService.SendEmail();


// List<Vehicle> vehicles = new List<Vehicle>();
// vehicles.Add(new Plane( "長榮","xx123", 2008 ));
// vehicles.Add(new Car("Toyota","Yaris Cross",2025,5));
// vehicles.Add(new Motorcycle("Honda","cub125",2023));
// vehicles.ForEach((vehicle) =>
// {
//    vehicle.Start();
//    vehicle.Stop();
// });



// Order order = new Order(new SmsSender());
// order.PlaceOrder();

// CompositionCar compositionCar = new CompositionCar();
// compositionCar.StartCar();

// DiscountCalculator discountCalculator = new DiscountCalculator();
// var value = discountCalculator.Calculate(new GoldDiscount(),100);
// Console.WriteLine(value);


// OrderService orderService = new OrderService(new SqlOrderRepository());
// orderService.ProcessOrder("測試");

// OrderService orderService2 = new OrderService(new MongoDbOrderRepostitory());
// orderService2.ProcessOrder("123");

// Document doc = new Document(new DraftState());

// doc.RequestPublish();
// Console.WriteLine("------");

// doc.RequestReview();
// doc.RequestPublish();

// Console.WriteLine("------");

// doc.RequestReview();


// decimal orderAmount = 1000m;
// IPaymentStrategy creditStrategy = new CreditCardStrategy();
// CheckoutContext checkout = new CheckoutContext(creditStrategy);
// checkout.ProcessCheckout(orderAmount);

// checkout.SetPaymentStrategy(new LinePayStrategy());
// checkout.ProcessCheckout(orderAmount);

// Library library = new Library();
// library.AddBook(new Book("設計模式"));
// library.AddBook(new Book("C# 深入淺出"));

// ITerator<Book> iterator = library.CreateIterator();
// Console.WriteLine("開始走訪圖書館裡的書");

// while(iterator.HasNext())
// {
//     Book currentBook = iterator.Next();
//     Console.WriteLine($"{currentBook.Title}");
// }

// Light livingRoomLight = new Light();
// ICommand lightOn = new LightOnCommand(livingRoomLight);
// ICommand lightOff = new LightOffCommand(livingRoomLight);

// RemoteControl remote = new RemoteControl();
// Console.WriteLine("--開啟電燈");
// remote.SetCommand(lightOn);
// remote.PressButton();

// Console.WriteLine("--關閉電燈--");
// remote.SetCommand(lightOff);
// remote.PressButton();

// Console.WriteLine("--復原剛才動作--");
// remote.PressUndoButton();
// remote.PressUndoButton();
// remote.PressUndoButton();
// remote.PressUndoButton();

// Beverage coffee = new Coffee();
// coffee.PrepareRecipe();

// Beverage tea = new Tea();
// tea.PrepareRecipe();


// var channel = new YouTubeChannel();
// var sub1 = new Subscriber("Jeff");
// var sub2 = new Subscriber("Hani");

// channel.Attach(sub1);
// channel.Attach(sub2);

// channel.UploadVideo("C#教學");

// channel.Detach(sub1);
// channel.UploadVideo("工程師必看");


// var charRoom = new ChatRoom();
// var alice = new MediatorUser("Alice");
// var bob = new MediatorUser("Bob");
// var jeff = new MediatorUser("Jeff");

// charRoom.RegisterUser(alice);
// charRoom.RegisterUser(bob);
// charRoom.RegisterUser(jeff);

// alice.Send("哈囉大家好");
// jeff.Send("嗨 Alice");
// jeff.SendPrivate("測試","Alice");

// 1. 建立各個節點
// Approver manager = new Manager();
// Approver director = new Director();
// Approver ceo = new CEO();
// 2. 組裝責任鏈：Manager -> Director -> CEO
// manager.SetNext(director).SetNext(ceo);
// 3. 發送請求 (用戶端只需要接觸責任鏈的「起點」)
// Console.WriteLine("--- 申請 500 元 ---");
// manager.ProcessRequest(500);
// Console.WriteLine("\n--- 申請 35,000 元 ---");
// manager.ProcessRequest(35000);
// Console.WriteLine("\n--- 申請 1,000,000 元 ---");
// manager.ProcessRequest(1000000);

// List<IEmployee> employees = new List<IEmployee>
// {
//     new Engineer("Alice",60000,10000),
//     new Engineer("Bob",50000,8000),
//     new DesignPatterns.Behavioral.Visitor.Manager("Hani",100000,3),
// };

// IVisitor bonusVistor = new BonusVisitor();
// IVisitor vacationVisitor = new VacationVisitor();
// Console.WriteLine("--年終獎金--");
// foreach(var employee in employees)
// {
//     employee.Accept(bonusVistor);
// }

// Console.WriteLine("\n--- 計算特休天數 ---");
// foreach(var employee in employees)
// {
//     employee.Accept(vacationVisitor);
// }

// Context context = new Context();
// context.Assign("a",10);
// context.Assign("b",5);
// context.Assign("c",2);

// IExpression a = new VariableExpression("a");
// IExpression b = new VariableExpression("b");
// IExpression c = new VariableExpression("c");

// IExpression expression = new SubtractExpression(new AddExpression(a,b),c);
// int result = expression.Interpret(context);
// Console.WriteLine($"Result of a + b - c  : {result}"); 

// DirectoryComposite rootDir = new DirectoryComposite("Root");
// FileLeaf file1 = new FileLeaf("config.xml");
// FileLeaf file2 = new FileLeaf("readme.txt");

// DirectoryComposite subDir = new DirectoryComposite("Images");
// FileLeaf img1 = new FileLeaf("logo.png");
// FileLeaf img2 = new FileLeaf("bg.jpg");
// subDir.Add(img1);
// subDir.Add(img2);

// rootDir.Add(file1);
// rootDir.Add(file2);
// rootDir.Add(subDir);

// rootDir.Display(1);

// 1. 建立一個舊有系統 (Adaptee) 的實例
LegacyXmlDataReader legacySystem = new LegacyXmlDataReader();

// 2. 舊系統因為只能吐出 XML，正常無法直接接到新系統上
// 建立一個適配器 (Adapter)，並將舊系統當作參數傳入進行封裝
IJsonDataProvider adapter = new XmlToJsonAdapter(legacySystem);

// 3. 客戶端 (Client) 只需認識 Target 介面 (IJsonDataProvider) 並調用方法
string resultJson = adapter.GetJsonData();

Console.WriteLine($"\n[Client] 成功取得最終資料:\n{resultJson}");