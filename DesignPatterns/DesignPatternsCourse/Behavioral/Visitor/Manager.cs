namespace DesignPatterns.Behavioral.Visitor
{
    public class Manager : IEmployee
    {
        public string Name {get;}
        public int Salary{get;}
        public int Subordinates{get;}

        public Manager(string name,int salary,int subordinates)
        {
            Name = name;
            Salary = salary;
            Subordinates = subordinates;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}