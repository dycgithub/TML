
using UnityEngine;

public class UIMethods 
{
    private static UIMethods instance;
    private static readonly object lockObj = new object();
    public static UIMethods GetInstance()
    {
        if (instance == null)
        {
            lock (lockObj)
            {
                if (instance == null)
                {
                    instance = new UIMethods();
                }
            }
        }
        return instance;
    }

    public GameObject FindCanvas()//查找Canvas
    {
        //TODO:多个画布
        GameObject canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
        if (canvas == null)
        {
            Debug.LogError("请设置Canvas");
        }
        return canvas;
    }
    public GameObject FindChild(GameObject parent, string childName)//查找子物体
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.name == childName)
            {
                return child.gameObject;
            }
        }
        Debug.LogError("未找到子物体: " + childName);
        return null;
    }

    public T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (component == null)
        {
            component = obj.AddComponent<T>();
            Debug.Log($"{obj.name} Add Component: {typeof(T)}");
        }
        return component;
    }
    public T GetOrAddSingleComponentInChild<T>(GameObject panel,string ComponentName) where T : Component
    {
        Transform[] transforms = panel.GetComponentsInChildren<Transform>();
        foreach (Transform tra in transforms)
        {
            if (tra.gameObject.name == ComponentName)
            {
                return tra.gameObject.GetComponent<T>();
                break;
            }
        }
        Debug.Log("Canvas not found: " + ComponentName);
        return null;
    }
    //TODO:从某个角度弹出
    //TODO:切换场景时清空UI
    //TODO:UI遮罩
    //TODO:UI动画private
    //TODO:UI点击
    //TODO:UI音效
}
