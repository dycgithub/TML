using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NormalShopUI : MonoBehaviour
{
    public GameObject item;
    public GameObject content;

    private void Start()
    {
        //TODO改为由服务器发送商品列表
        for (int i = 0; i < 100; i++)
        {
            var itemShop = GameObject.Instantiate(this.item);
            itemShop.transform.SetParent(content.transform);
            itemShop.transform.localPosition = Vector3.zero;
            itemShop.transform.localScale = Vector3.one;
            itemShop.name = $"ShopItem{i}";
            
            itemShop.transform.GetComponentInChildren<TMP_Text>().text=i.ToString();
            
            //var image = itemShop.transform.Find("Icon");//获取图标
            //image.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Icon/icon{i%51+1}");//51个图片取余会将多余的限制在范围内
            int index = i;
            itemShop.GetComponentInChildren<Button>().onClick.AddListener(()=>AddItemToInventory(index));

        }
        item.SetActive( false);
    }

    private void AddItemToInventory(int index)
    {
        GameObject inventory= GameObject.Find("Inventory");
        if (index<=2)
        {
            inventory.GetComponent<Inventory>().AddItem(index);
            Debug.Log("添加物品:{}   成功"+ index);
        }
        else
        {
            print("无法找到此物品{}"+ index);
        }
        
    }
}
