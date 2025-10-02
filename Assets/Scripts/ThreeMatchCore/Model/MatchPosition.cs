using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
/// <summary>
/// Model
/// 包含坐标、长度和方向等数据字段
/// 不依赖任何Unity组件（如 MonoBehaviour）
///不处理任何视觉表现或用户输入
/// </summary>
public class MatchPosition 
{
    public int2 coordinates;
    public int length;
    public bool isHorizontal;

    public MatchPosition(int x, int y, int length, bool isHorizontal)
    {
        coordinates.x = x;
        coordinates.y = y;
        this.length = length;
        this.isHorizontal = isHorizontal;
    }
}
