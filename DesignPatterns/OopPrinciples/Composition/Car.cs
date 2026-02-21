public class CompositionCar{
    private Chassis chassis = new Chassis();
    private Engine engine = new Engine();
    private Seats seats = new Seats();
    private Wheels wheels = new Wheels();
    public void StartCar()
    {
        engine.Start();
        wheels.Rotate();
        chassis.Support();
        seats.Sit();
        Console.WriteLine("車子啟動");
    }
}