namespace DesignPatterns.DesignPatternsCourse.Creational.Builder
{
    /// <summary>
    /// ConcreteBuilder B：電競旗艦機建造者。
    /// 
    /// 特性：
    ///   - 每個步驟都為旗艦效能做額外處理（如標注超頻、雙通道）
    ///   - 所有步驟都完整執行，包含 GPU
    ///   - GetResult() 後會重置內部 _quote，確保多次建造互不污染
    /// </summary>
    public class GamingComputerBuilder : IComputerBuilder
    {
        private ComputerQuote _quote = new ComputerQuote();

        public IComputerBuilder SetCPU(string cpu)
        {
            // 電競機強制標注超頻旗艦版本
            _quote.CPU = $"[超頻旗艦] {cpu}";
            return this;
        }

        public IComputerBuilder SetGPU(string gpu)
        {
            _quote.GPU = gpu;
            return this;
        }

        public IComputerBuilder SetRAM(string ram)
        {
            // 電競機強制雙通道配置
            _quote.RAM = $"{ram} (雙通道)";
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
