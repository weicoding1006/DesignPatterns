public class Coffee : Beverage
{
    protected override void Brew()
    {
        Console.WriteLine("用沸水沖泡咖啡粉");

    }
    protected override void AddCondiments()
    {
        Console.WriteLine("加入糖和牛奶");
    }

    protected override bool CustomerWantsCondiments()
    {
        return true;
    }
}