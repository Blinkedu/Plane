using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlane : PlaneBase
{
    public List<GameObject> bulletPrefabs;
    public Transform shootPoint;
    private int bigSkillMaxCount;
    private int bigSkillCount;
    private float bigSkillDuration;
    private float skillBuff;
    private float clearBulletCd;
    private float timer;
    private float clearBulletTimer;
    private Vector3 startPos;
    private Vector3 lastPos;
    private bool isMouseDown;
    private int maxHp = 0;
    private bool isRefreshPrg = false;
    private float bigSkillTimer = 0f;
    private bool isBigSkill = false;

    public override void Init(int hp, int atk, float shootRate, params object[] args)
    {
        if (args != null && args.Length > 0)
        {
            skillBuff = (float)args[0];
            clearBulletCd = (float)args[1];
            bigSkillMaxCount = bigSkillCount = (int)args[2];
            bigSkillDuration = (float)args[3];
        }
        else
        {
            skillBuff = 1f;
        }
        clearBulletTimer = 0;
        timer = shootRate;
        maxHp = hp;
        EventCenter.AddListener(EventDefine.ReleaseBigSkill, ReleaseBigSkill);
        EventCenter.AddListener(EventDefine.ResetClearBulletTimer, ResetClearBulletTimer);
        EventCenter.Broadcast<int, int>(EventDefine.UpdatePlayerHp, hp, maxHp);
        EventCenter.Broadcast<int, int>(EventDefine.UpdateBigSkillCount, bigSkillCount, bigSkillMaxCount);
        base.Init(hp, atk, shootRate, args);
    }

    protected override void Update()
    {
        base.Update();
        InputHandle();
        ClearBulletHandle();
        ShootHandle();
    }

    private void ClearBulletHandle()
    {
        clearBulletTimer += Time.deltaTime;
        if (clearBulletTimer < clearBulletCd)
        {
            EventCenter.Broadcast(EventDefine.UpdateClearBulletPrg, clearBulletTimer / clearBulletCd);
            isRefreshPrg = false;
        }
        else
        {
            if (!isRefreshPrg)
            {
                EventCenter.Broadcast(EventDefine.UpdateClearBulletPrg, clearBulletTimer / clearBulletCd);
                isRefreshPrg = true;
            }
        }
    }

    private void ShootHandle()
    {
        if (isBigSkill)
        {
            bigSkillTimer += Time.deltaTime;
            if (bigSkillTimer > bigSkillDuration)
            {
                bigSkillTimer = 0;
                isBigSkill = false;
                CancelInvoke("BigSkill");
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > shootRate)
            {
                Shoot();
                timer = 0;
            }
        }
    }

    public override void Hit(int dmg)
    {
        base.Hit(dmg);
        EventCenter.Broadcast<int, int>(EventDefine.UpdatePlayerHp, hp, maxHp);
    }

    public override void Die()
    {
        CreateDestroyEffect(Vector3.one);
        EventCenter.Broadcast<float>(EventDefine.UpdateClearBulletPrg, 0f);
        AudioManager.Instance.PlaySound(AudioConst.EFF_EXPLODE_2);
        GameManager.Instance.ChangeGameState(GameState.Over);
        base.Die();
    }

    protected override void OnShoot()
    {
        GameManager.Instance.Shoot(BulletType.Single, OwnerType.Player, shootPoint.position, 8, 1, atk);
    }

    private void InputHandle()
    {
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastPos = transform.position;
            isMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            startPos = Vector3.zero;
            isMouseDown = false;
        }

        if (isMouseDown)
        {
            Vector3 pos = lastPos + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPos);
            Vector3 targetPos = new Vector3(Mathf.Clamp(pos.x, -2.8f, 2.8f),
                Mathf.Clamp(pos.y, -4.8f, 4.8f),
                transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.3f);
        }
    }

    private void ResetClearBulletTimer()
    {
        clearBulletTimer = 0;
    }

    private void ReleaseBigSkill()
    {
        bigSkillCount -= 1;
        isBigSkill = true;
        EventCenter.Broadcast<int, int>(EventDefine.UpdateBigSkillCount, bigSkillCount, bigSkillMaxCount);
        InvokeRepeating("BigSkill", 0, 0.2f);
    }

    private void BigSkill()
    {
        GameManager.Instance.Shoot(BulletType.Sector, OwnerType.Player, shootPoint.position, 5, 35, (int)(atk * skillBuff));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ReleaseBigSkill, ReleaseBigSkill);
        EventCenter.RemoveListener(EventDefine.ResetClearBulletTimer, ResetClearBulletTimer);
    }
}
