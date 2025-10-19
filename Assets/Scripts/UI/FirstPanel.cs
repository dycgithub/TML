using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPanel : BasePanel
{
    private static string name="MainMenu";
    private static string path="UIStart/MainMenu";
    public static readonly UIType uiType=new UIType(path,name);
    
    public FirstPanel() : base(uiType)//将UI信息传到父类
    {
        
    }
    public override void OnStart()
    {
        //实例:为按钮添加监听事件
        UIMethods.GetInstance().GetOrAddSingleComponentInChild<Button>(activePanel, "Start").onClick.AddListener(StartGameInMain);
        UIMethods.GetInstance().GetOrAddSingleComponentInChild<Button>(activePanel, "Store").onClick.AddListener(ShopOpen);
        UIMethods.GetInstance().GetOrAddSingleComponentInChild<Button>(activePanel, "Exit").onClick.AddListener(ExitInMain);
        base.OnStart();
        Debug.Log("StartPanel OnStart");
    }

    private void StartGameInMain()
    {
        GameRoot.GetInstance().UI_Root.Pop(false);
        
    }
    private void ShopOpen()
    {
        GameRoot.GetInstance().UI_Root.Push(new SecondPanel());
        
    }
    private void ExitInMain()
    {
        Application.Quit();
    }
    
}
