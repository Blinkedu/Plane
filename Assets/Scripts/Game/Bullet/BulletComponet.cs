using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponet : MonoBehaviour
{
    private OwnerType owner;
    private float speed;
    private int count;
    private int atk;

    private bool isInited = false;

    public void Init(OwnerType owner, Vector3 pos, Vector3 eulerAngles, float speed, int atk)
    {
        this.owner = owner;
        this.speed = speed * (owner == OwnerType.Player ? 1f : -1f);
        this.atk = atk;
        transform.position = pos;
        transform.localEulerAngles = eulerAngles;
        isInited = true;
    }

    private void Update()
    {
        if (isInited)
        {
            transform.Translate(transform.up * speed * Time.deltaTime, Space.Self);
        }

        if (transform.position.y > 5f || transform.position.y < -6f)
        {
            DestroySelf();
        }
    }

    public void Recycle()
    {
        owner = OwnerType.None;
        speed = 0;
        atk = 0;
        isInited = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 伤害计算
        if (owner == OwnerType.Player)
        {
            if (collision.CompareTag("Enemy"))
            {
                var enemy = GameManager.Instance.GetEemey(collision.gameObject);
                if (enemy != null)
                    enemy.Hit(atk);
                DestroySelf();
            }
        }
        else if (owner == OwnerType.Enemy)
        {
            if (collision.CompareTag("Player"))
            {
                var player = GameManager.Instance.GetPlayerPlane();
                if (player != null)
                    player.Hit(atk);
                else
                    Debug.LogError("Player is null！");
                DestroySelf();
            }
        }
    }

    private void DestroySelf()
    {
        GameManager.Instance.RemoveBulletFromList(this);
        Destroy(this.gameObject);
    }
}
