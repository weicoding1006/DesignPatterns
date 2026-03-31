namespace DesignPatterns.Behavioral.Chain_of_Responsibility
{
    public class Manager : Approver
    {
        public override void ProcessRequest(int amount)
        {
            if(amount <= 1000)
            {
                Console.WriteLine($"Manager 核准了 {amount}元的申請。");
            }
            else if(_nextApprover != null)
            {
                Console.WriteLine("Manager權限不足，往上呈報...");
                _nextApprover.ProcessRequest(amount);
            }
        }
    }
}
