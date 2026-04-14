namespace DesignPatterns.DesignPatternsCourse.Creational.FactoryMethod
{
    /// <summary>
    /// ConcreteProduct B：輪船，走海路配送。
    /// </summary>
    public class Ship : ITransport
    {
        public void Deliver(string cargo)
        {
            Console.WriteLine($"[輪船🚢] 透過海路運送：{cargo}");
        }
    }
}
