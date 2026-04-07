namespace DesignPatterns.Structural.Composite;

public abstract class FileSystemComponent
{
    protected string name;

    public FileSystemComponent(string name)
    {
        this.name = name;
    }

    public abstract void Display(int depth);

    public virtual void Add(FileSystemComponent component)
    {
        throw new NotSupportedException("葉節點無法新增子節點。");
    }

    public virtual void Remove(FileSystemComponent component)
    {
        throw new NotSupportedException("葉節點無法移除子節點。");
    }
}
