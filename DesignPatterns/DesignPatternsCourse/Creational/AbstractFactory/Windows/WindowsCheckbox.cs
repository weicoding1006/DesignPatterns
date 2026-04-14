using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Abstracts;

namespace DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Windows
{
    public class WindowsCheckbox : ICheckbox
    {
        private bool _isChecked = false;

        public void Render()
        {
            Console.WriteLine($"  [Windows Checkbox] 方形核取方塊：{(_isChecked ? "[✓]" : "[ ]")}");
        }

        public void Toggle()
        {
            _isChecked = !_isChecked;
            Console.WriteLine($"  [Windows Checkbox] 切換為：{(_isChecked ? "已勾選 ✓" : "未勾選")}");
        }
    }
}
