using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Abstracts;

namespace DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Linux
{
    public class LinuxCheckbox : ICheckbox
    {
        private bool _isChecked = false;

        public void Render()
        {
            Console.WriteLine($"  [Linux Checkbox] GTK 核取方塊：{(_isChecked ? "[x]" : "[ ]")}");
        }

        public void Toggle()
        {
            _isChecked = !_isChecked;
            Console.WriteLine($"  [Linux Checkbox] 切換為：{(_isChecked ? "已選取 x" : "未選取")}");
        }
    }
}
