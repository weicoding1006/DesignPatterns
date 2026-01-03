public class Car : Vehicle
{
    public int NumberOfDoors { get; set; }
    public Car(string brand, string model, int year, int numberOfDoors) : base(brand, model, year)
    {
        NumberOfDoors = numberOfDoors;
    }
}