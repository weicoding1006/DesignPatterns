namespace DesignPatterns.Structural.Decorator
{
    // 具體裝飾者 (Concrete Decorator)：牛奶，加價 $15
    public class Milk : CondimentDecorator
    {
        public Milk(IBeverage beverage) : base(beverage) { }

        public override string GetDescription() => _beverage.GetDescription() + " +牛奶";
        public override double GetCost() => _beverage.GetCost() + 15.0;
    }
}
