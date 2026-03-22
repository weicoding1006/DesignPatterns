//Command (命令介面)
public interface ICommand
{
    void Execute();
    void Undo();
}