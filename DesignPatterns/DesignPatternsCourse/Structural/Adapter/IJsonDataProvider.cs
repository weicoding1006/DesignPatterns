namespace DesignPatterns.DesignPatternsCourse.Structural.Adapter
{
    /// <summary>
    /// Target 介面 (目標)：
    /// 這是客戶端 (Client) 所期望使用的介面。
    /// 在我們的情境中，這個現代的資料處理系統只接受 JSON 格式的資料。
    /// </summary>
    public interface IJsonDataProvider
    {
        string GetJsonData();
    }
}
