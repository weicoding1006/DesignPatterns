namespace DesignPatterns.Behavioral.Chain_of_Responsibility
{
    public class Director : Approver
    {
        public override void ProcessRequest(int amount)
        {
            if (amount <= 50000)
            {
                Console.WriteLine($"Director 核准了{amount}元的申請");
            }
            else if (_nextApprover != null)
            {
                Console.WriteLine("Director權限不足，往上呈報");
                _nextApprover.ProcessRequest(amount);
            }
        }
    }
}
