using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Abstracts;

namespace DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Mac
{
    public class MacButton : IButton
    {
        public void Render()
        {
            Console.WriteLine("  [Mac Button] 渲染圓角風格按鈕 ( OK )");
        }

        public void OnClick()
        {
            Console.WriteLine("  [Mac Button] 點擊：觸覺回饋 + 淡入淡出動畫");
        }
    }
}
