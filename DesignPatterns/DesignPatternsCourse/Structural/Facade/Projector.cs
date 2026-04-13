namespace DesignPatterns.Structural.Facade
{
    //投影機
    public class Projector
    {
        public void On() => Console.WriteLine("投影機:開機");
        public void SetInput(string input) => Console.WriteLine($"投影機:切換輸入源至:{input}");
        public void Off() => Console.WriteLine("投影機:關機");
    }
}