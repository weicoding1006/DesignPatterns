using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Abstracts;

namespace DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Linux
{
    public class LinuxUIFactory : IUIFactory
    {
        public IButton CreateButton()     => new LinuxButton();
        public ICheckbox CreateCheckbox() => new LinuxCheckbox();
    }
}
