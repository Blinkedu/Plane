using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public bool showOnAwake = false;

    protected virtual void Awake()
    {
        gameObject.SetActive(showOnAwake);
    }

    protected virtual void Show()
    {
        gameObject.SetActive(true);
    }

    protected virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
