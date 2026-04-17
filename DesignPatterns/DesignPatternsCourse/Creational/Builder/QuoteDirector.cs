namespace DesignPatterns.DesignPatternsCourse.Creational.Builder
{
    /// <summary>
    /// Director（指揮者）：封裝「建構步驟的呼叫順序與組合」。
    /// 
    /// Director 知道「做什麼步驟、按什麼順序」，
    /// 但不知道「每個步驟具體怎麼做」（那是 Builder 的職責）。
    /// 
    /// 優點：
    ///   - 呼叫方只需說「我要辦公機」或「我要電競機」，不需要一步步呼叫步驟
    ///   - 同一個 Director 方法，搭配不同的 Builder，可產出不同風格的成品
    ///   - 標準流程集中在 Director，修改流程只改這裡
    /// </summary>
    public class QuoteDirector
    {
        private IComputerBuilder _builder;

        public QuoteDirector(IComputerBuilder builder)
        {
            _builder = builder;
        }

        /// <summary>
        /// 允許在執行期切換建造者，
        /// 讓同一個 Director 實例可以建造不同種類的成品。
        /// </summary>
        public void SetBuilder(IComputerBuilder builder)
        {
            _builder = builder;
        }

        /// <summary>
        /// 標準辦公機建造流程：
        /// CPU → GPU（此步驟由 OfficeBuilder 自行跳過）→ RAM → Storage → PSU
        /// </summary>
        public ComputerQuote BuildOfficePC()
        {
            Console.WriteLine("  >> Director 執行辦公機建造流程");
            return _builder
                .SetCPU("Intel Core i5-14400")
                .SetGPU("（此參數由 Builder 決定是否採用）")
                .SetRAM("16GB DDR5")
                .SetStorage("512GB NVMe SSD")
                .SetPSU("500W 80+ Bronze")
                .GetResult();
        }

        /// <summary>
        /// 標準電競旗艦機建造流程：
        /// 所有零件都以頂規為目標
        /// </summary>
        public ComputerQuote BuildGamingPC()
        {
            Console.WriteLine("  >> Director 執行電競機建造流程");
            return _builder
                .SetCPU("Intel Core i9-14900K")
                .SetGPU("NVIDIA RTX 4090 24GB")
                .SetRAM("64GB DDR5")
                .SetStorage("2TB PCIe Gen5 NVMe SSD")
                .SetPSU("1000W 80+ Platinum")
                .GetResult();
        }
    }
}
