namespace DesignPatterns.Behavioral.Visitor
{
    public class BonusVisitor : IVisitor
    {
        public void Visit(Engineer engineer)
        {
            double bonus = engineer.Salary * 1.5 + engineer.CodesWritten * 0.1;
            Console.WriteLine($"{engineer.Name}(工程師)的獎金是:{bonus}");
        }

        public void Visit(Manager manager)
        {
            double bonus = manager.Salary * 2 + manager.Subordinates * 5000;
            Console.WriteLine($"{manager.Name}(Manager)的獎金是:{bonus}");
        }
    }
}