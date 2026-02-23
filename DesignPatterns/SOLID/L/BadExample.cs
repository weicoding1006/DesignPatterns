public class BadBird
{
    public virtual void Fly()
    {
        Console.WriteLine("這隻鳥在天空飛翔!");
    }
}

public class BadSparrow : BadBird
{
    //麻雀會飛很正常
}

public class BadPenguin : BadBird
{
    public override void Fly()
    {
        throw new NotImplementedException("企鵝不會飛");
    }
}