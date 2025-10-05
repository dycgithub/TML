using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbaseUI : MonoBehaviour
{
    public void SetUIRedPoint(Transform transform,int num)
    {
        if (num <= 0)
        {
            transform.Find("RedPoint").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("RedPoint").gameObject.SetActive(true);
            transform.Find("RedPoint/root/text").GetComponent<TMP_Text>().text = num.ToString();
        }
    }
}
public class Data
{
    public static string play = "play";
    public static string one = "1";
    public static string two = "2";
    public static string bag = "bag";
    public static string shop = "shop";
    public static string home = "home";
}