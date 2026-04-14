using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Abstracts;

namespace DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Windows
{
    public class WindowsButton : IButton
    {
        public void Render()
        {
            Console.WriteLine("  [Windows Button] 渲染矩形風格按鈕 [ OK ]");
        }

        public void OnClick()
        {
            Console.WriteLine("  [Windows Button] 點擊：播放 Windows 系統音效 *叮*");
        }
    }
}
