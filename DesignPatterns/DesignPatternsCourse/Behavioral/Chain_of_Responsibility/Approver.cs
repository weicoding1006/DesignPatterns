namespace DesignPatterns.Behavioral.Chain_of_Responsibility
{
    public abstract class Approver
    {
        protected Approver _nextApprover;
        public Approver SetNext(Approver nextApprover)
        {
            _nextApprover = nextApprover;
            return nextApprover;
        }

        public abstract void ProcessRequest(int amount);
    }
}
