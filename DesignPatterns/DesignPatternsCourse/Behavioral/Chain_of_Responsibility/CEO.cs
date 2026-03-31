namespace DesignPatterns.Behavioral.Chain_of_Responsibility
{
    public class CEO : Approver
    {
        public override void ProcessRequest(int amount)
        {
            Console.WriteLine($"CEO核准了{amount}元的申請");
        }
    }

}