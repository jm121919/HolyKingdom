using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
    }
}
