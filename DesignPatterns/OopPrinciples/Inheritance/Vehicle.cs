public class Vehicle
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }

    public Vehicle(string brand, string model, int year)
    {
        Brand = brand;
        Model = model;
        Year = year;
    }

    public void Start()
    {
        Console.WriteLine($"{Brand} {Model} is starting.");
    }

    public void Stop()
    {
        Console.WriteLine($"{Brand} {Model} is stopping.");
    }
}