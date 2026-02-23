public class BadDiscountCalculator
{
    public double CalculateDiscount(string customerType,double amount)
    {
        if(customerType == "Regular")
        {
            return amount * 0.05;
        }
        else if(customerType == "Gold")
        {
            return amount * 0.10;
        }
        return 0;
    }
}