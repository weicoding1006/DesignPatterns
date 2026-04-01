namespace DesignPatterns.Behavioral.Visitor
{
    public class Engineer : IEmployee
    {
        public string Name { get; }
        public int Salary { get; }
        public int CodesWritten { get; }
        public Engineer(string name,int salary,int codesWritten)
        {
            Name = name;
            Salary = salary;
            CodesWritten = codesWritten;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}