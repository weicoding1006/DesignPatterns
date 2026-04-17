namespace DesignPatternsCourse.Creational.Prototype;

/// <summary>
/// Prototype 介面：所有可被克隆的怪物原型都必須實作此介面。
/// <br/>
/// 設計決策：回傳 IMonsterPrototype 而非 object，
/// 讓呼叫方可以直接使用介面方法，無需強制轉型。
/// </summary>
public interface IMonsterPrototype
{
    /// <summary>
    /// 深拷貝：回傳一個與自身完全獨立的新物件。
    /// 修改回傳的 Clone 不會影響原型，也不會影響其他 Clone。
    /// </summary>
    IMonsterPrototype Clone();

    /// <summary>
    /// 設定怪物等級，並自動調整名稱與屬性。
    /// </summary>
    void SetLevel(int level);

    /// <summary>
    /// 展示怪物的完整配置資訊。
    /// </summary>
    void Show();
}
