using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCloseBullet : BulletBase
{
    public override void Shoot(OwnerType owner, Vector3 pos, float speed, int count, int atk, object args = null)
    {
        while (count > 0)
        {
            CreateBulletAndGetComponent(owner).Init(owner, new Vector3(Random.Range(pos.x - 0.5f, pos.x + 0.5f), 
                /*pos.y - 0.3f * count*/pos.y, pos.z),
                CalcEulerAngles(owner), Random.Range(speed - 3f, speed + 3f), atk);
            count--;
        }
    }
}