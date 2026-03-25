public abstract class Beverage
{
    public void PrepareRecipe()
    {
        BoilWater();
        Brew();
        PourInCup();

        if(CustomerWantsCondiments())
        {
            AddCondiments();
        }
    }

    private void BoilWater()
    {
        Console.WriteLine("把水煮沸");
    }

    private void PourInCup()
    {
        Console.WriteLine("倒進杯子裡");
    }

    protected abstract void Brew();
    protected abstract void AddCondiments();

    protected virtual bool CustomerWantsCondiments()
    {
        return false;
    }
}