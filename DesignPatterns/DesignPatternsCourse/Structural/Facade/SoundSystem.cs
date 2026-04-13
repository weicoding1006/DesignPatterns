namespace DesignPatterns.Structural.Facade
{
    public class SoundSystem
    {
        public void On() => Console.Write("音響:開機");
        public void SetVolume(int level) => Console.WriteLine($"音響:音量設定為{level}");
        public void Off() => Console.WriteLine("音響:關機");

    }
}