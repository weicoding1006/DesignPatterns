namespace DesignPatterns.DesignPatternsCourse.Creational.FactoryMethod
{
    /// <summary>
    /// Product 介面：定義所有運輸工具共同的行為。
    /// </summary>
    public interface ITransport
    {
        void Deliver(string cargo);
    }
}
