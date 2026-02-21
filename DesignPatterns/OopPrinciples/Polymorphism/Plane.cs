public class Plane : Vehicle
{
    public Plane(string brand,string model,int year) : base(brand,model,year)
    {}
    

    public override void Start()
    {
        Console.WriteLine("飛機正在飛");
    }

    public override void Stop()
    {
        Console.WriteLine("飛機停止");
    }
}