public class Bird
{
    public virtual void Eat()
    {
        Console.WriteLine("這隻鳥在吃東西");
    }    
}

public interface IFlyable
{
    void Fly();
}

public class Sparrow : Bird, IFlyable
{
    public void Fly()
    {
        Console.WriteLine("麻雀在天空飛");
    }
}

public class Penguin : Bird
{
    
}