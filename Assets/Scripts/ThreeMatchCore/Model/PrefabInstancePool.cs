using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Model
///PrefabInstancePool<T> 结构体负责管理对象池的数据结构
///通过 Stack<T> pool 存储和维护预制体实例
/// </summary>

public struct PrefabInstancePool<T> where T : MonoBehaviour
{
    Stack<T> pool;

    public T GetInstance (T prefab)
    {
        if (pool == null)
        {
            pool = new();
        }
#if UNITY_EDITOR
        else if (pool.TryPeek(out T i) && !i)
        {
            // Instances destroyed, assuming due to exiting play mode.
            pool.Clear();
        }
#endif
		
        if (pool.TryPop(out T instance))
        {
            instance.gameObject.SetActive(true);
        }
        else
        {
            instance = Object.Instantiate(prefab);
        }
        return instance;
    }

    public void Recycle (T instance)
    {
#if UNITY_EDITOR
        if (pool == null)
        {
            // Pool lost, assuming due to hot reload.
            Object.Destroy(instance.gameObject);
            return;
        }
#endif
        pool.Push(instance);
        instance.gameObject.SetActive(false);
    }
}