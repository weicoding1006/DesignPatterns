public class CheckoutContext
{
    private IPaymentStrategy _paymentStrategy;
    public CheckoutContext(IPaymentStrategy paymentStrategy)
    {
        _paymentStrategy = paymentStrategy;
    }

    public void SetPaymentStrategy(IPaymentStrategy paymentStrategy)
    {
        _paymentStrategy = paymentStrategy;
    }

    public void ProcessCheckout(decimal amount)
    {
        _paymentStrategy.Pay(amount);
    }
}