public class Tea : Beverage
{
    protected override void Brew()
    {
        Console.WriteLine("用沸水浸泡茶葉");
    }
    protected override void AddCondiments()
    {
        Console.WriteLine("加入檸檬");
    }
}