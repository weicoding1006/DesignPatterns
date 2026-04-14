using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Abstracts;

namespace DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory
{
    public class UIApplication
    {
        private readonly IButton _button;
        private readonly ICheckbox _checkbox;

        public UIApplication(IUIFactory factory)
        {
            _button = factory.CreateButton();
            _checkbox = factory.CreateCheckbox();
        }

        public void RenderUI()
        {
            Console.WriteLine("  >> 渲染介面元件");
            _button.Render();
            _checkbox.Render();
        }

        public void SimulateInteraction()
        {
            Console.WriteLine("  >> 模擬使用者互動");
            _button.OnClick();
            _checkbox.Toggle();
            _checkbox.Render();
        }
    }
}
