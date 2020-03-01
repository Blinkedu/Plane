/// <summary>
/// 游戏状态
/// </summary>
public enum GameState
{
    None = 0,
    Menu,
    Battle,
    Pause,
    Over
}

/// <summary>
/// 游戏模式
/// </summary>
public enum GameMode
{
    None = 0,
    Easy,
    Hard
}

/// <summary>
/// 子弹类型
/// </summary>
public enum BulletType
{
    None = 0,
    Single,     // 单个
    SingleCol,  // 单列
    RandomClose, // 随机密集
    Sector,     // 扇形
    Laser,      // 激光
}

/// <summary>
/// 拥有者类型
/// </summary>
public enum OwnerType
{
    None = 0,
    Player,
    Enemy
}