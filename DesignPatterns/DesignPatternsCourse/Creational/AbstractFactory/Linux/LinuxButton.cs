using DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Abstracts;

namespace DesignPatterns.DesignPatternsCourse.Creational.AbstractFactory.Linux
{
    public class LinuxButton : IButton
    {
        public void Render()  => Console.WriteLine("  [Linux Button] 渲染 GTK 風格按鈕");
        public void OnClick() => Console.WriteLine("  [Linux Button] 點擊：D-Bus 事件發送");
    }
}
