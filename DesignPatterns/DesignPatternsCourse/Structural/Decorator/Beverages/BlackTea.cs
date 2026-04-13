namespace DesignPatterns.Structural.Decorator
{
    // 具體飲料 (Concrete Component)：紅茶，基本價格 $30
    public class BlackTea : IBeverage
    {
        public string GetDescription() => "紅茶";
        public double GetCost() => 30.0;
    }
}
