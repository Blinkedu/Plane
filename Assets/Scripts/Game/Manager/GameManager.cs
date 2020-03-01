using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameMode gameMode = GameMode.None;
    public GameMode GameMode { get { return gameMode; } }

    [SerializeField]
    private GameState gameState = GameState.None;
    public GameState GameState { get { return gameState; } }

    [SerializeField]
    private int score = 0;
    public int Score { get { return score; } }

    //=======================出生位置================================
    public Transform playerSpawnPoint;
    public Transform enemySpwanPoint;
    //===============================================================

    //=======================子弹发射器==============================
    private SingleBullet singleBullet = new SingleBullet();
    private SingleColBullet singleColBullet = new SingleColBullet();
    private RandomCloseBullet randomCloseBullet = new RandomCloseBullet();
    private SectorBullet sectorBullet = new SectorBullet();
    //===============================================================

    private PlaneBase player = null;
    private List<PlaneBase> enemyList = new List<PlaneBase>();
    private List<BulletComponet> bulletList = new List<BulletComponet>();

    private float createEnemyTimer = 0f;
    private float createEnemyRate = 3f;

    private void Awake()
    {
        Instance = this;
        Screen.SetResolution( 576, 1024, false);
    }

    private void Start()
    {
        EventCenter.Broadcast(EventDefine.ShowStartPanel);
        ChangeGameState(GameState.Menu);
    }

    private void Update()
    {
        if (GameState == GameState.Battle)
        {
            CreateEnemy();
        }
    }

    public void InitGame(GameMode mode)
    {
        this.gameMode = mode;
        this.score = 0;
        EventCenter.Broadcast<int>(EventDefine.UpdateScore, 0);
        InitConfig();
        ClearEnemyList();
        CreatePlayer();
        EventCenter.Broadcast<GameMode>(EventDefine.UpdateLevel, GameMode);
    }

    public void InitConfig()
    {
        createEnemyRate = ConfigManager.Instance.GetLevelConfig(GetLevel(GameMode)).createEnemyRate;
    }

    public void EnemySpawn()
    {
        if (GameState != GameState.Battle) return;
        var enemyCfgs = ConfigManager.Instance.GetLevelEnemyAttr(GetLevel(GameMode));
        var cfg = enemyCfgs[Random.Range(0, enemyCfgs.Count)];
        Vector3 pos = new Vector3(Random.Range(-2.3f, 2.3f), enemySpwanPoint.position.y, enemySpwanPoint.position.z);
        GameObject go = ResManager.Instance.LoadPrefab(cfg.prefabPath, enemySpwanPoint);
        go.transform.position = pos;
        var plane = go.GetComponent<EnemyPlaneOne>();
        foreach (var item in cfg.bullets)
        {
            plane.AddBulletAttr(item);
        }
        plane.Init(cfg.hp, 1, cfg.shootRate, cfg.score);
        enemyList.Add(plane);
    }
    
    public void CreateEnemy()
    {
        createEnemyTimer -= Time.deltaTime;
        if (createEnemyTimer < 0f)
        {
            EnemySpawn();
            createEnemyTimer = createEnemyRate;
        }
        //InvokeRepeating("EnemySpawn", 1f, 3f);
    }

    public void CreatePlayer()
    {
        GameObject planeGo = ResManager.Instance.LoadPrefab(PrefabConst.PLAYER_PREFAB, playerSpawnPoint);
        planeGo.transform.position = playerSpawnPoint.position;
        player = planeGo.GetComponent<PlayerPlane>();
        var cfg = ConfigManager.Instance.GetLevelPlayerAttr(GetLevel(GameMode));
        player.Init(cfg.hp, cfg.atk, cfg.shootRate, cfg.skillBuff,cfg.clearBulletCd,cfg.bigSkillCount,cfg.bigSkillDuration);
    }

    public PlaneBase GetPlayerPlane()
    {
        return this.player;
    }

    public PlaneBase GetEemey(GameObject go)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
                i--;
            }
            else if (enemyList[i].gameObject == go)
            {
                return enemyList[i];
            }
        }
        return null;
    }

    public void RemoveEnemy(GameObject go)
    {
        var e = GetEemey(go);
        if (e != null)
            enemyList.Remove(e);
    }

    public void ClearEnemyList(bool isEffect = false)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (isEffect)
            {
                enemyList[i].Die();
            }
            else
            {
                Destroy(enemyList[i].gameObject);
            }
            i--;
        }
        enemyList.Clear();
    }

    public void ChangeGameState(GameState state)
    {
        this.gameState = state;
        switch (state)
        {
            case GameState.None:
                break;
            case GameState.Menu:
                Time.timeScale = 1;
                EventCenter.Broadcast(EventDefine.ShowStartPanel);
                AudioManager.Instance.PlayMusic(AudioConst.MENU_MUSIC);
                ClearGame();
                break;
            case GameState.Battle:
                Time.timeScale = 1;
                EventCenter.Broadcast(EventDefine.ShowBattlePanel);
                AudioManager.Instance.PlayMusic(AudioConst.BATTLE_MUSIC);
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                break;
            case GameState.Over:
                Time.timeScale = 1;
                EventCenter.Broadcast(EventDefine.ShowBattleResult);
                break;
            default:
                break;
        }
    }

    public void Shoot(BulletType bulletType, OwnerType owner, Vector3 pos, float speed, int count, int atk, object args = null)
    {
        switch (bulletType)
        {
            case BulletType.None:
                break;
            case BulletType.Single:
                singleBullet.Shoot(owner, pos, speed, 1, atk, args);
                break;
            case BulletType.SingleCol:
                singleColBullet.Shoot(owner, pos, speed, count, atk, args);
                break;
            case BulletType.RandomClose:
                randomCloseBullet.Shoot(owner, pos, speed, count, atk, args);
                break;
            case BulletType.Sector:
                sectorBullet.Shoot(owner, pos, speed, count, atk, args);
                break;
            case BulletType.Laser:
                break;
            default:
                break;
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
        EventCenter.Broadcast<int>(EventDefine.UpdateScore, Score);
    }

    public void AddBulletToList(BulletComponet bullet)
    {
        bulletList.Add(bullet);
    }

    public void RemoveBulletFromList(BulletComponet bullet)
    {
        bulletList.Remove(bullet);
    }

    public void ClearGame()
    {
        foreach (var item in enemyList)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        enemyList.Clear();
        foreach (var item in bulletList)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        bulletList.Clear();
        if (player != null)
        {
            Destroy(player.gameObject);
        }
    }

    // 0-所有，1-玩家，2-敌人
    public void ClearBullet(int type)
    {
        foreach (var item in bulletList)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
    }

    public int GetLevel(GameMode mode)
    {
        return GameMode == GameMode.Easy ? 1 : 2;
    }
}
