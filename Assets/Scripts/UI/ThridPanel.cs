using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThridPanel : BasePanel
{
    private static string name="GameingPanel";
    private static string path="UIStart/GameingPanel";
    public static readonly UIType uiType=new UIType(path,name);
    
    public ThridPanel() : base(uiType)//将UI信息传到父类
    {
        
    }
    
}
