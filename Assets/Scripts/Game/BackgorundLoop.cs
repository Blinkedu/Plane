using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgorundLoop : MonoBehaviour
{
    public float speed = 2f;
    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Battle) return;
        this.transform.Translate(Vector3.down * speed * Time.deltaTime);
        Vector3 postion = this.transform.position;
        if (postion.y <= -11.5f)
        {
            this.transform.position = new Vector3(postion.x, postion.y + 11.5f * 2, postion.z);
        }
    }
}
