namespace DesignPatterns.DesignPatternsCourse.Creational.Builder
{
    /// <summary>
    /// Product（成品）：最終被建造出的複雜物件 — 電腦報價單。
    /// 
    /// 所有屬性都有安全的預設值，由建造者依需求逐步填入。
    /// Product 本身不包含任何「如何被組裝」的邏輯，職責單純。
    /// </summary>
    public class ComputerQuote
    {
        public string CPU     { get; set; } = "（未配置）";
        public string GPU     { get; set; } = "（內顯）";
        public string RAM     { get; set; } = "（未配置）";
        public string Storage { get; set; } = "（未配置）";
        public string PSU     { get; set; } = "（未配置）";

        /// <summary>
        /// 以格式化表格顯示報價單內容。
        /// </summary>
        public void Show(string title)
        {
            Console.WriteLine($"  ┌─── {title} ───────────────────");
            Console.WriteLine($"  │  CPU     : {CPU}");
            Console.WriteLine($"  │  GPU     : {GPU}");
            Console.WriteLine($"  │  RAM     : {RAM}");
            Console.WriteLine($"  │  Storage : {Storage}");
            Console.WriteLine($"  │  PSU     : {PSU}");
            Console.WriteLine($"  └──────────────────────────────────");
        }
    }
}
