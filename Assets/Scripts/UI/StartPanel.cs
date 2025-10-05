using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    private static string name="StartPanel";
    private static string path="UIStart/StartPanel";
    public static readonly UIType uiType=new UIType(path,name);
    
    public StartPanel() : base(uiType)//将UI信息传到父类
    {
        
    }

    public override void OnStart()
    {
        //实例:为按钮添加监听事件
        UIMethods.GetInstance().GetOrAddSingleComponentInChild< Button>(activePanel,"middle").onClick.AddListener(backk);
        base.OnStart();
        Debug.Log("StartPanel OnStart");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("StartPanel OnEnable");
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Debug.Log("StartPanel OnDisable");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log("StartPanel OnDestroy");
    }
    
    //示例方法
    private void backk()
    {
        GameRoot.GetInstance().UI_Root.Pop(false);
    }
}
