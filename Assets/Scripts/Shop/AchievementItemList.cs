using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItemList : RecyclingListViewItem
{
    // 添加你需要的UI组件引用
    public TMP_Text itemNameText;
    public Image itemIcon;
    public Button buyButton;
    
    // 设置数据显示的方法
    public void SetData(ItemData data)
    {
        itemNameText.text = data.name;
        //itemIcon.sprite = data.icon;
        // 配置其他UI元素
    }
}
