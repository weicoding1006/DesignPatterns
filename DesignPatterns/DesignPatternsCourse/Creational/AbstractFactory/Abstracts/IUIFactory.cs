namespace DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Abstracts
{
    public interface IUIFactory
    {
        IButton CreateButton();
        ICheckbox CreateCheckbox();
    }
}
