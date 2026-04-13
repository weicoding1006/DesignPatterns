namespace DesignPatterns.Structural.Decorator
{
    public abstract class CondimentDecorator : IBeverage
    {
        protected IBeverage _beverage;
        public CondimentDecorator(IBeverage beverage)
        {
            _beverage = beverage;
        }
        public virtual double GetCost()
        {
            return _beverage.GetCost();
        }

        public virtual string GetDescription()
        {
            return _beverage.GetDescription();
        }
    }
}