using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : BasePanel
{
    private static string name="StartPanel";
    private static string path="StartPanel/StartPanel";
    public static readonly UIType uiType=new UIType(name,path);
    
    public StartPanel() : base(uiType)//将UI信息传到父类
    {
        
    }

    public override void OnStart()
    {
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
}
