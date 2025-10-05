using System;
using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// 从json读取库存配置
/// </summary>
public class ItemDatabase : MonoBehaviour {
    private List<Item> database = new List<Item>();
    private JsonData itemData;
    public bool Constructed = false;
    void Start()
    {
        
        
        	
    }

    private void OnEnable()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        Constructed=ConstructItemDatabase();
    }

    public Item FetchItemById(int id)//查找物品
    {
        Debug.Log("Fetching item with id: " + id);
        print(database.Count);
        for (int i = 0; i < database.Count; i++)
        {
            print(id);
            print(database[i].Id);
            
            if (database[i].Id == id)
            {
                return database[i];
            }
        }

        return null;
    }
	
    bool ConstructItemDatabase()//构建物品数据库
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            Item newItem = new Item();
            newItem.Id = (int)itemData[i]["id"];
            newItem.Title = itemData[i]["title"].ToString();
            newItem.Value = (int)itemData[i]["value"];
            newItem.Power = (int)itemData[i]["stats"]["power"];
            newItem.Defense = (int)itemData[i]["stats"]["defense"];
            newItem.Vitality = (int)itemData[i]["stats"]["vitality"];
            newItem.Description = itemData[i]["description"].ToString();
            newItem.Stackable = (bool)itemData[i]["stackable"];
            newItem.Rarity = (int)itemData[i]["rarity"];
            newItem.Slug = itemData[i]["slug"].ToString();
            newItem.Sprite = Resources.Load<Sprite>("Sprites/Items/" + newItem.Slug);
            print("构建中"+database.Count);
            database.Add(newItem);
        }
        return true;
    }
}

public class Item
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public int Power { get; set; }
    public int Defense { get; set; }
    public int Vitality { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }// 是否可堆叠
    public int Rarity { get; set; }
    public string Slug { get; set; }// 图片名称
    public Sprite Sprite { get; set; }

    public Item()
    {
        this.Id = -1;
    }
}