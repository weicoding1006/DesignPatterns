namespace DesignPatterns.Structural.Facade
{
    public class Lights
    {
        public void Dim(int level) => Console.Write($"燈光:調整至{level}%");
        public void On() => Console.WriteLine("燈光:全亮");
    }
}