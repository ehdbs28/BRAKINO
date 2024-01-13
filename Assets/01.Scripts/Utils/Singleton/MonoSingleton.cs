using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool shutdown = false;
    private static Object locker = new Object();
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (shutdown)
            {
                Debug.LogError($"[Singleton] Instance {typeof(T)} already destroyed. Returning null");
                return null;
            }

            lock (locker)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                }
            }

            return instance;
        }
    }

    public virtual void Init()
    {
        shutdown = false;
    }

    private void OnDisable()
    {
        shutdown = true;
    }

    private void OnDestroy()
    {
        shutdown = true;
    }
}