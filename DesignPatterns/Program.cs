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

CompositionCar compositionCar = new CompositionCar();
compositionCar.StartCar();