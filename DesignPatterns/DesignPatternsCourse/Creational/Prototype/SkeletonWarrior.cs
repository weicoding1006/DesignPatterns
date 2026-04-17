namespace DesignPatternsCourse.Creational.Prototype;

/// <summary>
/// 掉落物品表（巢狀物件）
/// <br/>
/// SkeletonWarrior 持有此型別，示範「物件內含自訂類別」的深拷貝情境。
/// </summary>
public class LootTable
{
    public List<string> Items      { get; set; } = new();
    public int          GoldAmount { get; set; } = 0;

    /// <summary>
    /// LootTable 本身的深拷貝方法，供持有它的物件在 Clone() 時呼叫。
    /// </summary>
    public LootTable DeepCopy()
    {
        return new LootTable
        {
            Items      = new List<string>(Items), // 複製 List
            GoldAmount = GoldAmount               // int 是值型別，直接複製即可
        };
    }
}

/// <summary>
/// 具體原型 B：骷髏士兵
/// <br/>
/// 進階深拷貝情境：物件內含「巢狀自訂類別」(LootTable)，
/// 需要對巢狀物件也執行深拷貝，否則 Clone 的 LootTable 仍共用原型的資料。
/// </summary>
public class SkeletonWarrior : IMonsterPrototype
{
    public string      Name    { get; set; } = "骷髏士兵";
    public string      Race    { get; set; } = "不死族";
    public MonsterSize Size    { get; set; } = MonsterSize.Medium;
    public int         BaseHP  { get; set; } = 80;
    public int         BaseATK { get; set; } = 18;
    public int         Level   { get; set; } = 1;

    public List<string> Resistances { get; set; } = new() { "物理減傷 30%", "不死免疫" };

    // LootTable 是自訂類別（參考型別），必須呼叫它的 DeepCopy()
    public LootTable Loot { get; set; } = new()
    {
        Items      = new List<string> { "骨頭", "生鏽短劍" },
        GoldAmount = 15
    };

    /// <inheritdoc/>
    public IMonsterPrototype Clone()
    {
        var clone = (SkeletonWarrior)MemberwiseClone();

        // 深拷貝 List
        clone.Resistances = new List<string>(Resistances);

        // 深拷貝巢狀自訂物件——這是與 Goblin 範例的關鍵差異點
        clone.Loot = Loot.DeepCopy();

        return clone;
    }

    /// <inheritdoc/>
    public void SetLevel(int level)
    {
        Level = level;
        // 等級 >= 10 晉升為「骷髏騎士」
        Name  = level >= 10
            ? $"骷髏騎士（Lv.{level}）"
            : $"骷髏士兵（Lv.{level}）";

        // 高等級才解鎖稀有掉落（避免重複加入）
        if (level >= 10 && !Loot.Items.Contains("古代劍"))
            Loot.Items.Add("古代劍");
    }

    /// <inheritdoc/>
    public void Show()
    {
        Console.WriteLine($"  ┌─── {Name} ───────────────────");
        Console.WriteLine($"  │  種族  : {Race}");
        Console.WriteLine($"  │  體型  : {Size}");
        Console.WriteLine($"  │  HP    : {BaseHP + Level * 15}  (Base {BaseHP} + Lv.{Level}×15)");
        Console.WriteLine($"  │  ATK   : {BaseATK + Level * 3}  (Base {BaseATK} + Lv.{Level}×3)");
        Console.WriteLine($"  │  抗性  : {string.Join(", ", Resistances)}");
        Console.WriteLine($"  │  掉落  : {string.Join(", ", Loot.Items)} + {Loot.GoldAmount} 金幣");
        Console.WriteLine($"  └──────────────────────────────────");
    }
}
