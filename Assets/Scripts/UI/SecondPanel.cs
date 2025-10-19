using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondPanel : BasePanel
{
    private static string name="StorePanel";
    private static string path="UIStart/StorePanel";
    public static readonly UIType uiType=new UIType(path,name);
    
    public SecondPanel() : base(uiType)//将UI信息传到父类
    {
        
    }
    public override void OnStart()
    {
        //实例:为按钮添加监听事件
        UIMethods.GetInstance().GetOrAddSingleComponentInChild<Button>(activePanel,"BackMain").onClick.AddListener(BackMainMenu);
        base.OnStart();
        Debug.Log("StartPanel OnStart");
    }
    private void BackMainMenu()
    {
        GameRoot.GetInstance().UI_Root.Pop(false);
        
    }
    
}
