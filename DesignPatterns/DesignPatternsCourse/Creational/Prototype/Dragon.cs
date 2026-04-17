namespace DesignPatternsCourse.Creational.Prototype;

/// <summary>
/// 具體原型 C：龍（BOSS 等級）
/// <br/>
/// 示範含有多個 List 欄位（Resistances、Skills、Drops）的深拷貝，
/// 以及等級提升時自動解鎖新技能的動態原型行為。
/// </summary>
public class Dragon : IMonsterPrototype
{
    public string      Name    { get; set; } = "古龍";
    public string      Race    { get; set; } = "龍族";
    public MonsterSize Size    { get; set; } = MonsterSize.Large;
    public int         BaseHP  { get; set; } = 500;
    public int         BaseATK { get; set; } = 80;
    public int         Level   { get; set; } = 1;

    public List<string> Resistances { get; set; } = new() { "火屬性免疫", "物理減傷 50%" };
    public List<string> Skills      { get; set; } = new() { "爪擊", "烈焰吐息", "尾掃" };
    public List<string> Drops       { get; set; } = new() { "龍鱗", "龍牙", "龍心石" };

    /// <inheritdoc/>
    public IMonsterPrototype Clone()
    {
        var clone = (Dragon)MemberwiseClone();

        // 每個 List 都需要獨立深拷貝
        clone.Resistances = new List<string>(Resistances);
        clone.Skills      = new List<string>(Skills);
        clone.Drops       = new List<string>(Drops);

        return clone;
    }

    /// <inheritdoc/>
    public void SetLevel(int level)
    {
        Level = level;
        // 等級 >= 50 晉升為「神話古龍」
        Name  = level >= 50
            ? $"神話古龍（Lv.{level}）"
            : $"古龍（Lv.{level}）";

        // 解鎖等級限定技能（避免重複加入）
        if (level >= 30 && !Skills.Contains("龍息衝波"))
            Skills.Add("龍息衝波");
        if (level >= 50 && !Skills.Contains("世界終焉"))
            Skills.Add("世界終焉");
    }

    /// <inheritdoc/>
    public void Show()
    {
        Console.WriteLine($"  ┌─── {Name} ───────────────────");
        Console.WriteLine($"  │  種族  : {Race}");
        Console.WriteLine($"  │  體型  : {Size}");
        Console.WriteLine($"  │  HP    : {BaseHP + Level * 50}  (Base {BaseHP} + Lv.{Level}×50)");
        Console.WriteLine($"  │  ATK   : {BaseATK + Level * 10}  (Base {BaseATK} + Lv.{Level}×10)");
        Console.WriteLine($"  │  抗性  : {string.Join(", ", Resistances)}");
        Console.WriteLine($"  │  技能  : {string.Join(", ", Skills)}");
        Console.WriteLine($"  │  掉落  : {string.Join(", ", Drops)}");
        Console.WriteLine($"  └──────────────────────────────────");
    }
}
