using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Item item;
    private string data;
    private GameObject tooltip;

    void Start()
    {
        tooltip = GameObject.Find("Tooltip");
        tooltip.SetActive(false);
    }

    void Update()
    {
        if (tooltip.activeSelf)//跟随鼠标
        {
            tooltip.transform.position = Input.mousePosition;
        }
    }

    public void Activate(Item item)//显示物品信息
    {
        this.item = item;
        ConstructDataString();
        tooltip.SetActive(true);
    }

    public void Deactivate()
    {
        tooltip.SetActive(false);
    }//构造显示字符串

    public void ConstructDataString()
    {
        data = "<color=#FFEC58FF><b>" + item.Title + "</b></color>\n\n" + item.Description
               + "\nPower: " + item.Power;
        tooltip.transform.GetChild(0).GetComponent<TMP_Text>().text = data;
    }//显示隐藏

}