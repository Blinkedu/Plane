using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorBullet : BulletBase
{
    public override void Shoot(OwnerType owner, Vector3 pos, float speed, int count, int atk, object args = null)
    {
        float angle = CalcAngle(args);
        for (int i = -(count / 2); i <= count / 2; i++)
        {
            CreateBulletAndGetComponent(owner).Init(owner, pos, new Vector3(0, 0, i * angle + (owner == OwnerType.Player ? 0f : 180f)), speed, atk);
        }
    }

    private float CalcAngle(object arg)
    {
        if (arg == null)
            return 3f;
        else
            return (float)arg;
    }
}
