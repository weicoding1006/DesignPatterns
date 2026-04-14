namespace DesignPatterns.DesignPatternsCourse.Creational.FactoryMethod
{
    /// <summary>
    /// ConcreteCreator A：陸路物流公司。
    /// 覆寫工廠方法，決定建立「卡車」。
    /// </summary>
    public class RoadLogistics : LogisticsCreator
    {
        public override ITransport CreateTransport()
        {
            return new Truck();
        }
    }
}
