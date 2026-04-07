namespace DesignPatterns.DesignPatternsCourse.Structural.Adapter
{
    /// <summary>
    /// Adaptee 類別 (被適配者)：
    /// 這是已經存在、且具有實用功能的類別或第三方函式庫。
    /// 但它的介面與我們目前系統的標準介面 (這裡指 IJsonDataProvider) 不相容。
    /// 在這個例子中，它只提供 XML 格式的資料。
    /// </summary>
    public class LegacyXmlDataReader
    {
        public string GetXmlData()
        {
            // 模擬從舊系統或伺服器獲取 XML 資料
            return "<?xml version='1.0'?><Data><Item>Adapter Pattern</Item><Item>Structural Design</Item></Data>";
        }
    }
}
