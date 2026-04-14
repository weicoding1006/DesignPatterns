namespace DesignPatterns.DesignPatternsCourse.Creational.Singleton
{
    public sealed class AppConfig
    {
        private static readonly AppConfig _instance = new AppConfig();
        public static AppConfig Instance => _instance;

        public string ApplicationName {get;private set;}
        public string Version{get;private set;}

        private AppConfig()
        {
            ApplicationName = "設計模式教學";
            Version = "1.0.0";
        }

        public void UpdateVersion(string version)
        {
            Version = version;
        }
    }
}