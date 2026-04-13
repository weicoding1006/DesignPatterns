namespace DesignPatterns.DesignPatternsCourse.Structural.Flyweight
{
    public class Tree
    {
        private int _x;
        private int _y;
        private TreeType _type;
        public Tree(int x, int y, TreeType type)
        {
            _x = x;
            _y = y;
            _type = type;
        }

        public void Draw() => _type.Draw(_x, _y);
    }
}