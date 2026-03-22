public class RemoteControl
{
    private ICommand _command;
    private readonly Stack<ICommand> _history = new Stack<ICommand>();

    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public void PressButton()
    {
        if(_command != null)
        {
            _command.Execute();
            _history.Push(_command);
        }
    }

    public void PressUndoButton()
    {
        if(_history.Count > 0)
        {
            ICommand lastCommand = _history.Pop();
            lastCommand.Undo();
        }
        else
        {
            Console.WriteLine("沒有可以復原的動作了");
        }
    }
}