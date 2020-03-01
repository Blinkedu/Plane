using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaneOne : PlaneBase
{
    public Transform shootPoint;
    public float speed = 1f;
    private float timer = 0f;
    private int score = 0;
    protected List<BulletAttribute> bulletAttrList = new List<BulletAttribute>();

    public override void Init(int hp, int atk, float shootRate, params object[] args)
    {
        base.Init(hp, atk, shootRate, args);
        if (args != null && args.Length > 0)
        {
            score = (int)args[0];
        }
    }

    protected override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        if (timer > shootRate)
        {
            Shoot();
            timer = 0;
        }
        transform.Translate(-transform.up * Time.deltaTime * speed, Space.Self);
        if (transform.position.y < -8f)
            Destroy(gameObject);
    }

    protected override void OnShoot()
    {
        var attr = GetRandomBulletAttr();
        GameManager.Instance.Shoot(attr.type, OwnerType.Enemy, shootPoint.position, attr.speed, attr.count, 1);
    }

    public override void Die()
    {
        CreateDestroyEffect(new Vector3(0.6f, 0.6f, 1f));
        AudioManager.Instance.PlaySound(AudioConst.EFF_EXPLODE_1);
        GameManager.Instance.AddScore(score);
        base.Die();
    }

    public virtual void AddBulletAttr(params BulletAttribute[] bullets)
    {
        if (bullets == null || bullets.Length == 0)
        {
            this.bulletAttrList.Add(new BulletAttribute()
            {
                type = BulletType.Single,
                count = 1,
                speed = 7,
                angle = 0
            });
        }
        else
        {
            foreach (var item in bullets)
            {
                this.bulletAttrList.Add(item);
            }
        }
    }

    protected BulletAttribute GetRandomBulletAttr()
    {
        return bulletAttrList[Random.Range(0, bulletAttrList.Count)];
    }

}
