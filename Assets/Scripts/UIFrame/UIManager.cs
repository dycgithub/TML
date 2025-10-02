using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 单例和堆栈存储UI元素
/// 由root创建实例
/// </summary>
public class UIManager 
{
    private static UIManager instance;
    public Stack<BasePanel> stackUi;//使用栈存储UI元素
    public Dictionary<string,GameObject>dictUiObject;//panel名称与obj的对应关系
    public GameObject canvas;//
    
    public static UIManager GetInstance()
    {
        if (instance == null)
        {
            return instance;
        }
        else
        {
            return instance;
        }
        
    }

    public UIManager()
    {
        instance = this;
        stackUi = new Stack<BasePanel>();
        dictUiObject = new Dictionary<string, GameObject>();
    }
    public GameObject GetSingleObject(UIType uiType)//获取UI对象
    {
        if (dictUiObject.ContainsKey(uiType.Name))//已经存在
        {
            return dictUiObject[uiType.Name];
        }
        if (!canvas) canvas=UIMethods.GetInstance().FindCanvas();
        GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(uiType.Path), canvas.transform);//根据路径加载UI
        return obj;
        
    }
    /// <summary>
    /// 入栈
    /// </summary>
    /// <param name="panel"></param>
    public void Push(BasePanel panel)
    {
        if(stackUi.Count>0)
        {
            stackUi.Peek().OnDisable();//暂停栈顶元素
        }
        GameObject uiObj=GetSingleObject(panel.uiType);
        dictUiObject.Add(panel.uiType.Name,uiObj);
        panel.activePanel = uiObj;//设置当前panel的激活对象
        if(stackUi.Count==0)
        {
            stackUi.Push(panel);//栈为空直接入栈
        }
        else
        {
            if (stackUi.Peek().uiType.Name == panel.uiType.Name)//相同UI不入栈
            {
                return;
            }
            else
            {
                stackUi.Push(panel);
            }
        }
        panel.OnStart();
    }
/// <summary>
/// 出栈
/// </summary>
/// <param name="isLoad"></param>
    public void Pop(bool isLoad)
    {
        if (isLoad)
        {
            while (stackUi.Count > 0)
            {
                stackUi.Peek().OnDisable();
                stackUi.Peek().OnDestroy();
                GameObject.Destroy(dictUiObject[stackUi.Peek().uiType.Name]);
                dictUiObject.Remove(stackUi.Peek().uiType.Name);
                stackUi.Pop();
            }
        }
        else
        {
            if (stackUi.Count > 0)
            {
                stackUi.Peek().OnDisable();
                stackUi.Peek().OnDestroy();
                GameObject.Destroy(dictUiObject[stackUi.Peek().uiType.Name]);
                dictUiObject.Remove(stackUi.Peek().uiType.Name);
                stackUi.Pop();
                if(stackUi.Count>0)
                {
                    stackUi.Peek().OnEnable();
                }
            }
        }
    }
    
    
    
}
