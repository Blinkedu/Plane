using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase
{
    public abstract void Shoot(OwnerType owner, Vector3 pos, float speed, int count, int atk, object args = null);

    protected virtual Vector3 CalcEulerAngles(OwnerType owner)
    {
        if (owner == OwnerType.Player)
            return Vector3.zero;
        else
            return new Vector3(0, 0, 180);
    }

    protected virtual BulletComponet CreateBulletAndGetComponent(OwnerType owner)
    {
        GameObject bulletGo = owner == OwnerType.Player ?
            ResManager.Instance.LoadPrefab(PrefabConst.PLAYER_BULLET) : 
            ResManager.Instance.LoadPrefab(PrefabConst.ENEMY_BULLET);
        var bullet = bulletGo.AddComponent<BulletComponet>();
        GameManager.Instance.AddBulletToList(bullet);
        return bullet;
    }
}
