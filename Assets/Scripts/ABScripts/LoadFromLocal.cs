using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFromLocal : MonoBehaviour
{
    private void Start()
    {
        AssetBundle AB = AssetBundle.LoadFromFile("AssetBundle");
        GameObject obj = AB.LoadAsset<GameObject>("");
        Instantiate(obj);
    }
    
}
