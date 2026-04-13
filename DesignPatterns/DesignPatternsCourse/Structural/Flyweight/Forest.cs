namespace DesignPatterns.DesignPatternsCourse.Structural.Flyweight
{
    /// <summary>
    /// Forest 是 Client，負責管理所有 Tree（持有外在狀態 x, y）
    /// </summary>
    public class Forest
    {
        private List<Tree> _trees = new List<Tree>();
        public int TreeCount => _trees.Count;

        public void PlantTree(int x, int y, string name, string color, string texture)
        {
            // 透過 Factory 取得共享的 TreeType（內在狀態）
            TreeType type = TreeFactory.GetTreeType(name, color, texture);
            _trees.Add(new Tree(x, y, type));
        }

        public void Draw()
        {
            foreach (var tree in _trees)
                tree.Draw();
        }
    }
}
