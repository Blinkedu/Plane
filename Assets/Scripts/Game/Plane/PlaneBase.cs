using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBase : MonoBehaviour
{
    [SerializeField]
    protected int hp;
    [SerializeField]
    protected int atk;
    [SerializeField]
    protected float shootRate;

    private bool isInited = false;

    public virtual void Init(int hp, int atk, float shootRate,params object[] args)
    {
        this.hp = hp;
        this.atk = atk;
        this.shootRate = shootRate;
        isInited = true;
    }

    protected virtual void Update()
    {
        if (!isInited) return;
    }

    public void Shoot()
    {
        if(GameManager.Instance.GameState == GameState.Battle)
        {
            OnShoot();
        }
    }

    protected virtual void OnShoot() { }

    public virtual void Hit(int dmg)
    {
        hp -= dmg;
        if(hp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        GameManager.Instance.RemoveEnemy(this.gameObject);
        Destroy(this.gameObject);
    }

    protected virtual GameObject CreateDestroyEffect(Vector3 scale)
    {
        GameObject effect = ResManager.Instance.LoadPrefab(PrefabConst.DESTROY_EFFECT);
        effect.transform.localScale = scale;
        effect.transform.position = transform.position;
        Destroy(effect, 1.5f);
        return effect;
    }

}