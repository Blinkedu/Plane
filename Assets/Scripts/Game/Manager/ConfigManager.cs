using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance { get; private set; }

    [Header("==============关卡配置==============")]
    public List<LevelConfig> levelConfigs;

    private void Awake()
    {
        Instance = this;
    }

    public LevelConfig GetLevelConfig(int level)
    {
        if(level - 1 >= levelConfigs.Count)
        {
            Debug.LogError("GetLevelConfig Error: 关卡超出索引范围！level=" + level);
            return default(LevelConfig);
        }
        return levelConfigs[level - 1];
    }

    public List<EnemyAttribute> GetLevelEnemyAttr(int level)
    {
        return GetLevelConfig(level).enemys;
    }

    public PlayerAttrbute GetLevelPlayerAttr(int level)
    {
        return GetLevelConfig(level).player;
    }
}

[System.Serializable]
public struct LevelConfig
{
    public PlayerAttrbute player;
    public float createEnemyRate;
    [Header("Boss在游戏开始后多久出场(单位:秒)")]
    public float bossAppearTime;// boss出场时间
    public List<EnemyAttribute> enemys;
}

[System.Serializable]
public struct EnemyAttribute
{
    public string prefabPath;
    public int hp;
    public int score;
    public int speed;
    public float shootRate;
    public List<BulletAttribute> bullets;
}

[System.Serializable]
public struct BulletAttribute
{
    public BulletType type;
    public int count;
    public float speed; 
    public float angle;
}

[System.Serializable]
public struct PlayerAttrbute
{
    public int hp;
    public int atk;
    public int bigSkillCount;
    [Header("大招持续时长")]
    public float bigSkillDuration;
    [Header("弹幕清空冷却时长")]
    public float clearBulletCd;
    public float skillBuff;
    public float shootRate;
}
