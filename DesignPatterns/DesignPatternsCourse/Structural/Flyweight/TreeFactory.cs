namespace DesignPatterns.DesignPatternsCourse.Structural.Flyweight
{
    public class TreeFactory
    {
        private static Dictionary<string, TreeType> _treeTypes = new Dictionary<string, TreeType>();
        public static TreeType GetTreeType(string name, string color, string texture)
        {
            string key = $"{name}_{color}_{texture}";
            if (!_treeTypes.ContainsKey(key))
            {
                _treeTypes[key] = new TreeType(name, color, texture);
                Console.WriteLine($"\n--- 享元工廠: 創建了新的樹木種類: {name} ---");
            }
            return _treeTypes[key];
        }

        public static int GetCacheSize()
        {
            return _treeTypes.Count;
        }
    }
}