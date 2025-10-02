using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIType : MonoBehaviour
{
    private string path;
    private string name;

    public string Path
    {
        get => path;
        set => path = value;
    }

    public string Name
    {
        get => name;
        set => name = value;
    }
/// <summary>
/// 获取UI信息
/// </summary>
/// <param name="_path   Panel的路径"></param>
/// <param name="_name   Panel的名称"></param>
    public UIType(string _path, string _name)
    {
        this.path = _path;
        this.name = _name;
    }
    
}
