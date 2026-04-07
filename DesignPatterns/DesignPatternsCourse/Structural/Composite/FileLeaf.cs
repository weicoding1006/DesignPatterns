namespace DesignPatterns.Structural.Composite;

public class FileLeaf : FileSystemComponent
{
    public FileLeaf(string name) : base(name) { }

    public override void Display(int depth)
    {
        Console.WriteLine(new String('-', depth) + " 檔案: " + name);
    }
}
