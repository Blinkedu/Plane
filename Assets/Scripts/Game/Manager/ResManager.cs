using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager
{
    private static ResManager _instance = null;
    public static ResManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ResManager();
            }
            return _instance;
        }
    }

    private Dictionary<string, UnityEngine.Object> resDic = new Dictionary<string, Object>();

    public T Load<T>(string path) where T : UnityEngine.Object
    {
        Object obj = null;
        if(resDic.TryGetValue(path,out obj) && obj != null)
        {
            return obj as T;
        }
        obj = Resources.Load<T>(path);
        resDic[path] = obj;
        return obj as T;
    }

    public GameObject LoadPrefab(string path,Transform parent = null)
    {
        GameObject prefab = Load<GameObject>(path);
        if (prefab != null)
        {
            return GameObject.Instantiate(prefab, parent);
        }
        return null;
    }
}
