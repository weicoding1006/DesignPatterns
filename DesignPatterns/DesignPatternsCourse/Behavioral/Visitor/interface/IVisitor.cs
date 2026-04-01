namespace DesignPatterns.Behavioral.Visitor
{
    public interface IVisitor
    {
        void Visit(Engineer engineer);
        void Visit(Manager manager);
    }
}
