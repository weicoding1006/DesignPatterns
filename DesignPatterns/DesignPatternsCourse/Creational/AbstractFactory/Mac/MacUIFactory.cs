using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Abstracts;

namespace DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Mac
{
    public class MacUIFactory : IUIFactory
    {
        public IButton CreateButton() => new MacButton();
        public ICheckbox CreateCheckbox() => new MacCheckbox();
    }
}
