namespace DesignPatterns.Behavioral.Visitor
{
    public interface IEmployee
    {
        void Accept(IVisitor visitor);
        string Name { get; }
        int Salary { get; }
    }
}