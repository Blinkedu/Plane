using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleColBullet : BulletBase
{
    public override void Shoot(OwnerType owner, Vector3 pos, float speed, int count, int atk, object args = null)
    {
        while (count > 0)
        {
            CreateBulletAndGetComponent(owner).Init(owner, new Vector3(pos.x, pos.y - 0.3f * count, pos.z), CalcEulerAngles(owner), speed, atk);
            count--;
        }
    }
}
