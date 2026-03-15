using System.Numerics;
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

Library library = new Library();
library.AddBook(new Book("設計模式"));
library.AddBook(new Book("C# 深入淺出"));

ITerator<Book> iterator = library.CreateIterator();
Console.WriteLine("開始走訪圖書館裡的書");

while(iterator.HasNext())
{
    Book currentBook = iterator.Next();
    Console.WriteLine($"{currentBook.Title}");
}