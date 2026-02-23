public class DiscountCalculator
{
    public double Calculate(IDiscountStrategy discountStrategy,double amount)
    {
        return discountStrategy.CalculateDiscount(amount);
    }
}