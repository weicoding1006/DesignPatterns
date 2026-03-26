public interface ISubject
{
    void Attach(IObserver observer); //訂閱
    void Detach(IObserver observer); //取消訂閱
    void Notify(); //通知所有訂閱者
}