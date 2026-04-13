namespace DesignPatterns.Structural.Facade
{
    public class HomeTheaterFacade
    {
        private readonly Projector _projector;
        private readonly SoundSystem _sound;
        private readonly Lights _lights;

        public HomeTheaterFacade(Projector projector,SoundSystem sound,Lights lights)
        {
            _projector = projector;
            _sound = sound;
            _lights = lights;
        }

        public void WatchMovie()
        {
            Console.WriteLine("\n --準備看電影 --");
            _lights.Dim(10);
            _projector.On();
            _projector.SetInput("HDMI");
            _sound.On();
            _sound.SetVolume(50);
        }

        public void EndMovie()
        {
            Console.WriteLine("\n --電影結束，關閉系統--");
            _projector.Off();
            _sound.Off();
            _lights.On();
        }
    }
}