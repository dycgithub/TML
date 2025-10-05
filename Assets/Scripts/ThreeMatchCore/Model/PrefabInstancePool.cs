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
		
        if (pool.TryPop(out T instance))//尝试从栈中弹出一个元素
        {
            instance.gameObject.SetActive(true);
        }
        else
        {
            instance = Object.Instantiate(prefab);//池中没有对象则实例化
        }
        return instance;
    }

    public void Recycle (T instance)
    {
#if UNITY_EDITOR
        if (pool == null)
        {
            // Pool丢失，假设是由于热重载造成的
            Object.Destroy(instance.gameObject);
            return;
        }
#endif
        pool.Push(instance);//添加
        instance.gameObject.SetActive(false);//禁用
    }
}