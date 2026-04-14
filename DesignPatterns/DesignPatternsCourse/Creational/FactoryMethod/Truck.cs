namespace DesignPatterns.DesignPatternsCourse.Creational.FactoryMethod
{
    /// <summary>
    /// ConcreteProduct A：卡車，走陸路配送。
    /// </summary>
    public class Truck : ITransport
    {
        public void Deliver(string cargo)
        {
            Console.WriteLine($"[卡車🚛] 透過公路運送：{cargo}");
        }
    }
}
