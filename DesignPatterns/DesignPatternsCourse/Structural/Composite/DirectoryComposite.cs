using System;
using System.Collections.Generic;

namespace DesignPatterns.Structural.Composite;

public class DirectoryComposite : FileSystemComponent
{
    private List<FileSystemComponent> _children = new List<FileSystemComponent>();

    public DirectoryComposite(string name) : base(name) { }

    public override void Add(FileSystemComponent component)
    {
        _children.Add(component);
    }

    public override void Remove(FileSystemComponent component)
    {
        _children.Remove(component);
    }

    public override void Display(int depth)
    {
        Console.WriteLine(new String('-', depth) + "+ 資料夾: " + name);

        foreach (FileSystemComponent component in _children)
        {
            component.Display(depth + 2);
        }
    }
}
