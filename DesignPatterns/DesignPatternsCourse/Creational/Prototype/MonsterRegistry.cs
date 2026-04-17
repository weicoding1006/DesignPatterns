namespace DesignPatternsCourse.Creational.Prototype;

/// <summary>
/// 原型登錄表（Prototype Registry）
/// <br/>
/// 職責：
/// 1. 管理所有已登錄的原型物件（以字串 key 為索引）
/// 2. 提供統一的 Spawn(key) 入口，對外 Clone 原型
/// <br/>
/// 設計效益：
/// - 呼叫方完全不感知任何具體怪物類別（Goblin、Dragon…）
/// - 新增怪物類型只需呼叫 Register()，呼叫方的 Spawn() 程式碼不用改動
/// - 滿足開閉原則（OCP）：對擴展開放，對修改封閉
/// </summary>
public class MonsterRegistry
{
    // key = 怪物種類識別字串，value = 已初始化完成的原型物件
    private readonly Dictionary<string, IMonsterPrototype> _prototypes = new();

    /// <summary>
    /// 將一個初始化完成的原型物件登錄到 Registry。
    /// </summary>
    /// <param name="key">識別此原型的字串 key（如 "goblin"、"dragon"）</param>
    /// <param name="prototype">已完整初始化的原型物件</param>
    public void Register(string key, IMonsterPrototype prototype)
    {
        _prototypes[key] = prototype;
        Console.WriteLine($"  [Registry] 已登錄原型：{key}");
    }

    /// <summary>
    /// 根據 key Clone 對應的原型，回傳一個獨立的新怪物物件。
    /// <br/>
    /// 若 key 不存在，拋出 KeyNotFoundException，讓問題在開發期間提早暴露。
    /// </summary>
    /// <param name="key">與 Register() 對應的字串 key</param>
    /// <returns>Clone 出的獨立怪物物件，修改它不會影響原型或其他 Clone</returns>
    public IMonsterPrototype Spawn(string key)
    {
        if (!_prototypes.TryGetValue(key, out var prototype))
            throw new KeyNotFoundException(
                $"找不到怪物原型：'{key}'，請先呼叫 Register() 登錄此識別字串");

        // 核心：永遠 Clone，永遠不把原型本身交給呼叫方
        return prototype.Clone();
    }
}
