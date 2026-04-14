using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Abstracts;

namespace DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Windows
{
    public class WindowsUIFactory : IUIFactory
    {
        public IButton CreateButton() => new WindowsButton();
        public ICheckbox CreateCheckbox() => new WindowsCheckbox();
    }
}
