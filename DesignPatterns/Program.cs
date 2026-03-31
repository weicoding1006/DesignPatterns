using System.Numerics;
using DesignPatterns.Behavioral.Chain_of_Responsibility;
using DesignPatterns.OopPrinciples.Encapsulation;

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
Approver manager = new Manager();
Approver director = new Director();
Approver ceo = new CEO();
// 2. 組裝責任鏈：Manager -> Director -> CEO
manager.SetNext(director).SetNext(ceo);
// 3. 發送請求 (用戶端只需要接觸責任鏈的「起點」)
Console.WriteLine("--- 申請 500 元 ---");
manager.ProcessRequest(500);
Console.WriteLine("\n--- 申請 35,000 元 ---");
manager.ProcessRequest(35000);
Console.WriteLine("\n--- 申請 1,000,000 元 ---");
manager.ProcessRequest(1000000);