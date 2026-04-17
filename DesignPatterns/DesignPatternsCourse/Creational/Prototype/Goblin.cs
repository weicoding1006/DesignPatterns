namespace DesignPatternsCourse.Creational.Prototype;

/// <summary>
/// 具體原型 A：哥布林
/// <br/>
/// 深拷貝策略：
/// 1. 呼叫 MemberwiseClone() 快速複製所有值型別欄位（int、enum）
///    以及 string（string 是 immutable，共用指標安全）
/// 2. 手動 new 一份新的 List{T}，切斷與原型的共用關係
/// </summary>
public class Goblin : IMonsterPrototype
{
    public string      Name        { get; set; } = "哥布林";
    public string      Race        { get; set; } = "哥布林族";
    public MonsterSize Size        { get; set; } = MonsterSize.Small;
    public int         BaseHP      { get; set; } = 50;
    public int         BaseATK     { get; set; } = 10;
    public int         Level       { get; set; } = 1;

    // List<T> 是參考型別，若使用淺拷貝（MemberwiseClone 預設行為），
    // Clone 與原型會共用同一個 List 物件，修改其中一個會影響另一個。
    // 因此必須深拷貝：用 new List<string>(source) 建立獨立副本。
    public List<string> Resistances { get; set; } = new() { "暗屬性" };
    public List<string> Drops       { get; set; } = new() { "哥布林耳朵", "銅幣" };

    /// <inheritdoc/>
    public IMonsterPrototype Clone()
    {
        // Step 1：淺拷貝——複製所有欄位（值型別 + string 指標）
        var clone = (Goblin)MemberwiseClone();

        // Step 2：深拷貝 List，確保 Clone 的 List 是獨立副本
        clone.Resistances = new List<string>(Resistances);
        clone.Drops       = new List<string>(Drops);

        return clone;
    }

    /// <inheritdoc/>
    public void SetLevel(int level)
    {
        Level = level;
        // 等級 >= 5 自動晉升為「菁英」
        Name  = level >= 5
            ? $"哥布林菁英（Lv.{level}）"
            : $"哥布林（Lv.{level}）";
    }

    /// <inheritdoc/>
    public void Show()
    {
        Console.WriteLine($"  ┌─── {Name} ───────────────────");
        Console.WriteLine($"  │  種族  : {Race}");
        Console.WriteLine($"  │  體型  : {Size}");
        Console.WriteLine($"  │  HP    : {BaseHP + Level * 10}  (Base {BaseHP} + Lv.{Level}×10)");
        Console.WriteLine($"  │  ATK   : {BaseATK + Level * 2}  (Base {BaseATK} + Lv.{Level}×2)");
        Console.WriteLine($"  │  抗性  : {string.Join(", ", Resistances)}");
        Console.WriteLine($"  │  掉落  : {string.Join(", ", Drops)}");
        Console.WriteLine($"  └──────────────────────────────────");
    }
}
