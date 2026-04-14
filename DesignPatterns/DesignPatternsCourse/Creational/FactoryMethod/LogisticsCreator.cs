namespace DesignPatterns.DesignPatternsCourse.Creational.FactoryMethod
{
    /// <summary>
    /// Creator（抽象創建者）：物流公司基底類別。
    /// 定義了完整的業務流程（PlanDelivery），
    /// 但「要建立哪種運輸工具」的決定權交給子類別。
    /// </summary>
    public abstract class LogisticsCreator
    {
        // 工廠方法：宣告建立產品的介面，由子類別決定具體實作。
        public abstract ITransport CreateTransport();

        // 高層業務邏輯：使用工廠方法建立的產品來完成配送任務。
        // 呼叫方只需呼叫此方法，完全不需要知道用的是卡車還是輪船。
        public void PlanDelivery(string cargo)
        {
            Console.WriteLine("--- 開始規劃配送 ---");
            ITransport transport = CreateTransport(); // 呼叫工廠方法取得產品
            transport.Deliver(cargo);
            Console.WriteLine("--- 配送完成 ---\n");
        }
    }
}
