using System;
using System.Xml.Linq;
using System.Linq;

namespace DesignPatterns.DesignPatternsCourse.Structural.Adapter
{
    /// <summary>
    /// Adapter 類別 (適配器)：
    /// 負責將 Adaptee (LegacyXmlDataReader) 的介面轉換成 Client 所期望的 Target (IJsonDataProvider) 介面。
    /// 這裡我們實作的是「物件適配器 (Object Adapter)」，使用組合的方式包裝 Adaptee。
    /// </summary>
    public class XmlToJsonAdapter : IJsonDataProvider
    {
        private readonly LegacyXmlDataReader _legacyXmlReader;

        public XmlToJsonAdapter(LegacyXmlDataReader legacyXmlReader)
        {
            // 透過建構子注入 Adaptee 物件
            _legacyXmlReader = legacyXmlReader;
        }

        public string GetJsonData()
        {
            // 1. 從 Adaptee 獲取不相容格式的資料 (XML)
            string xmlData = _legacyXmlReader.GetXmlData();
            
            Console.WriteLine("[Adapter] 已從 Legacy 系統取得 XML 資料，正在進行轉換...");
            
            // 2. 在這將 XML 轉換成 JSON
            // 為了教學展示，我們簡易地解析 XML 轉字串。實務上會使用 Newtonsoft.Json 等套件進行轉換。
            var xmlDoc = XDocument.Parse(xmlData);
            var items = xmlDoc.Descendants("Item").Select(x => $"\"{x.Value}\"");
            var jsonStr = $"{{ \"Data\": [{string.Join(", ", items)}] }}";
            
            Console.WriteLine("[Adapter] 轉換完成！");

            // 3. 返回符合 Target 介面的資料
            return jsonStr;
        }
    }
}
