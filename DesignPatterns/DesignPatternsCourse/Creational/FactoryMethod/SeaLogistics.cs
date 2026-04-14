namespace DesignPatterns.DesignPatternsCourse.Creational.FactoryMethod
{
    /// <summary>
    /// ConcreteCreator B：海路物流公司。
    /// 覆寫工廠方法，決定建立「輪船」。
    /// </summary>
    public class SeaLogistics : LogisticsCreator
    {
        public override ITransport CreateTransport()
        {
            return new Ship();
        }
    }
}
