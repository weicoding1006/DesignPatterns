namespace DesignPatterns.Structural.Decorator
{
    // 具體飲料 (Concrete Component)：綠茶，基本價格 $25
    public class GreenTea : IBeverage
    {
        public string GetDescription() => "綠茶";
        public double GetCost() => 25.0;
    }
}
