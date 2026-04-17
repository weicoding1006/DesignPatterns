namespace DesignPatterns.DesignPatternsCourse.Creational.Builder
{
    /// <summary>
    /// ConcreteBuilder A：辦公文書機建造者。
    /// 
    /// 特性：
    ///   - 所有步驟都著重「夠用就好」的實用規格
    ///   - SetGPU() 步驟被忽略（辦公機使用 CPU 內顯即可）
    ///   - GetResult() 後會重置內部 _quote，確保多次建造互不污染
    /// </summary>
    public class OfficeComputerBuilder : IComputerBuilder
    {
        private ComputerQuote _quote = new ComputerQuote();

        public IComputerBuilder SetCPU(string cpu)
        {
            _quote.CPU = cpu;
            return this;
        }

        public IComputerBuilder SetGPU(string gpu)
        {
            // 辦公機不需要獨立顯卡，直接忽略此步驟
            // Director 仍會呼叫此方法，但 Builder 自行決定如何回應
            Console.WriteLine("  [OfficeBuilder] 辦公機不需要獨立顯卡，跳過 GPU 步驟");
            return this;
        }

        public IComputerBuilder SetRAM(string ram)
        {
            _quote.RAM = ram;
            return this;
        }

        public IComputerBuilder SetStorage(string storage)
        {
            _quote.Storage = storage;
            return this;
        }

        public IComputerBuilder SetPSU(string psu)
        {
            _quote.PSU = psu;
            return this;
        }

        public ComputerQuote GetResult()
        {
            var result = _quote;
            _quote = new ComputerQuote(); // Reset，準備下一次建造
            return result;
        }
    }
}
