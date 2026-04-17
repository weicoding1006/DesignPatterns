namespace DesignPatterns.DesignPatternsCourse.Creational.Builder
{
    /// <summary>
    /// Builder Interface（建造者介面）：宣告所有組裝步驟的抽象方法。
    /// 
    /// 每個方法回傳 IComputerBuilder（this），支援 Fluent 鏈式呼叫，
    /// 讓呼叫方可以寫出：builder.SetCPU("...").SetRAM("...").GetResult()
    /// </summary>
    public interface IComputerBuilder
    {
        IComputerBuilder SetCPU(string cpu);
        IComputerBuilder SetGPU(string gpu);
        IComputerBuilder SetRAM(string ram);
        IComputerBuilder SetStorage(string storage);
        IComputerBuilder SetPSU(string psu);

        /// <summary>取得組裝完成的成品，並重置內部狀態以準備下一次建造。</summary>
        ComputerQuote GetResult();
    }
}
