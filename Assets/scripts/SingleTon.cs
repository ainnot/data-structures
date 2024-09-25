using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon <T>: MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
                if (Instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }
    public virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
