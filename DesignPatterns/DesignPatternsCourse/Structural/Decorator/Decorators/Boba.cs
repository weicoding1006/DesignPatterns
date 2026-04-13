namespace DesignPatterns.Structural.Decorator
{
    // 具體裝飾者 (Concrete Decorator)：珍珠，加價 $10
    public class Boba : CondimentDecorator
    {
        public Boba(IBeverage beverage) : base(beverage) { }

        public override string GetDescription() => _beverage.GetDescription() + " +珍珠";
        public override double GetCost() => _beverage.GetCost() + 10.0;
    }
}
