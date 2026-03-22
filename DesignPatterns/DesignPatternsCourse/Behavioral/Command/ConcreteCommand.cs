public class LightOnCommand : ICommand
{
    private readonly Light _light;
    public LightOnCommand(Light light)
    {
        _light = light;
    }
    public void Execute()
    {
        _light.TurnOn();
    }

    public void Undo()
    {
        _light.TurnOff();
    }
}

public class LightOffCommand : ICommand
{
    private readonly Light _light;
    public LightOffCommand(Light light)
    {
        _light = light;
    }
    public void Execute()
    {
        _light.TurnOff();
    }

    public void Undo()
    {
        _light.TurnOn();
    }
}