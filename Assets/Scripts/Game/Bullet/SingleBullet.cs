using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBullet : BulletBase
{
    public override void Shoot(OwnerType owner, Vector3 pos, float speed, int count, int atk, object args = null)
    {
        if (owner == OwnerType.Player)
            AudioManager.Instance.PlaySound(AudioConst.EFF_BULLET);
        CreateBulletAndGetComponent(owner).Init(owner, pos, CalcEulerAngles(owner), speed, atk);
    }
}