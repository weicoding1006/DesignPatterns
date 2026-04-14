using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Abstracts;

namespace DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Mac
{
    public class MacCheckbox : ICheckbox
    {
        private bool _isOn = false;

        public void Render()
        {
            Console.WriteLine($"  [Mac Checkbox] 圓形切換開關：{(_isOn ? "● ON " : "○ OFF")} （滑動動畫）");
        }

        public void Toggle()
        {
            _isOn = !_isOn;
            Console.WriteLine($"  [Mac Checkbox] 切換為：{(_isOn ? "ON ●" : "OFF ○")}");
        }
    }
}
