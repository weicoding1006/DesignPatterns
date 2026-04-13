namespace DesignPatterns.DesignPatternsCourse.Structural.Flyweight
{
    public class TreeType
    {
        public string Name { get; private set; }
        public string Color { get; private set; }
        public string Texture { get; private set; }

        public TreeType(string name,string color,string texture)
        {
            Name = name;
            Color = color;
            Texture = texture;
        }

        public void Draw(int x, int y)
        {
            Console.WriteLine($"繪製樹木:[品種={Name},顏色={Color},紋理={Texture}] 於座標({x},{y})");
        }
    }
}
