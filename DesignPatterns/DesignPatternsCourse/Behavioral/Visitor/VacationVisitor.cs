namespace DesignPatterns.Behavioral.Visitor
{
    public class VacationVisitor : IVisitor
    {
        public void Visit(Engineer engineer)
        {
            Console.WriteLine($"{engineer.Name}(工程師)有14天特休");
        }

        public void Visit(Manager manager)
        {
            Console.WriteLine($"{manager.Name}(Manager)有21天特休");
        }
    }
}