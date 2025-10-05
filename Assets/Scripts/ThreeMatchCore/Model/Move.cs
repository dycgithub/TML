using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;

/// <summary>
///Model
///使用 [System.Serializable] 特性，便于在不同层之间传输数据
///Move 结构体用于存储和表示移动操作的数据，包括起始位置 From、目标位置 To 和移动方向 Direction
///是一个纯粹的数据载体，不涉及任何UI渲染或用户交互
///	业务逻辑封装
///FindMove 方法封装了寻找可行移动的算法逻辑
///通过分析游戏状态来计算可能的移动，属于核心业务逻辑
/// </summary>


[System.Serializable]
public struct Move
{
    public MoveDirection Direction { get; private set; }
    public int2 From { get; private set; }
    public int2 To { get; private set; }
    public bool IsValid => Direction != MoveDirection.None;//属性，判断移动是否合法

    public Move(int2 coordinates, MoveDirection direction)//构造函数
    {
        Direction = direction;
        From = coordinates;
        To = coordinates + direction switch
        {
            MoveDirection.Up => int2(0, 1),
            MoveDirection.Right => int2(1, 0),
            MoveDirection.Down => int2(0, -1),
            _ => int2(-1, 0)
        };
        #region 上面的写法等同于下面的写法
        // int2 directionVector;
        // switch (direction)
        // {
        //     case MoveDirection.Up:
        //         directionVector = int2(0, 1);
        //         break;
        //     case MoveDirection.Right:
        //         directionVector = int2(1, 0);
        //         break;
        //     case MoveDirection.Down:
        //         directionVector = int2(0, -1);
        //         break;
        //     default:
        //         directionVector = int2(-1, 0);
        //         break;
        // }
        // To = coordinates + directionVector;
        

        #endregion
    }

    public static Move FindMove (Match3GamePlay game)
	{
		int2 s = game.Size;//获取游戏区域的尺寸
		for (int2 c = 0; c.y < s.y; c.y++)
		{
			for (c.x = 0; c.x < s.x; c.x++)//遍历所有块
			{
				TileState t = game[c];//获取当前块的类型
				//0010
				if (c.x >= 3 && game[c.x - 2, c.y] == t && game[c.x - 3, c.y] == t)//向左查找
				{
					return new Move(c, MoveDirection.Left);
				}
				//0100
				if (c.x + 3 < s.x && game[c.x + 2, c.y] == t && game[c.x + 3, c.y] == t)//向右查找
				{
					return new Move(c, MoveDirection.Right);
				}
				//0
				//1
				//0
				//0
				if (c.y >= 3 && game[c.x, c.y - 2] == t && game[c.x, c.y - 3] == t)//向下查找
				{
					return new Move(c, MoveDirection.Down);
				}
				//0
				//0
				//1
				//0
				if (c.y + 3 < s.y && game[c.x, c.y + 2] == t && game[c.x, c.y + 3] == t)//向上查找
				{
					return new Move(c, MoveDirection.Up);
				}
				
				if (c.y > 1)
				{
					if (c.x > 1 && game[c.x - 1, c.y - 1] == t)
					{
						//  0
						//0010
						if (c.x >= 2 && game[c.x - 2, c.y - 1] == t || c.x + 1 < s.x && game[c.x + 1, c.y - 1] == t)
						{
							return new Move(c, MoveDirection.Down);//四横
						}
						//0
						//0
						//10
						//0
						if (c.y >= 2 && game[c.x - 1, c.y - 2] == t || c.y + 1 < s.y && game[c.x - 1, c.y + 1] == t)
						{
							return new Move(c, MoveDirection.Left);
						}
					}
					
					if (c.x + 1 < s.x && game[c.x + 1, c.y - 1] == t)
					{
						//0
						//100
						if (c.x + 2 < s.x && game[c.x + 2, c.y - 1] == t)
						{
							return new Move(c, MoveDirection.Down);
						}
						//01
						// 0
						// 0
						if (c.y >= 2 && game[c.x + 1, c.y - 2] == t || c.y + 1 < s.y && game[c.x + 1, c.y + 1] == t)
						{
							return new Move(c, MoveDirection.Right);
						}
					}
				}
				
				if (c.y + 1 < s.y)
				{
					if (c.x > 1 && game[c.x - 1, c.y + 1] == t)
					{
						//
						if (c.x >= 2 && game[c.x - 2, c.y + 1] == t || c.x + 1 < s.x && game[c.x + 1, c.y + 1] == t)	
						{
							return new Move(c, MoveDirection.Up);
						}
						//
						if (c.y + 2 < s.y && game[c.x - 1, c.y + 2] == t)
						{
							return new Move(c, MoveDirection.Left);
						}
					}

					if (c.x + 1 < s.x && game[c.x + 1, c.y + 1] == t)
					{
						//
						if (c.x + 2 < s.x && game[c.x + 2, c.y + 1] == t)
						{
							return new Move(c, MoveDirection.Up);
						}
						//
						if (c.y + 2 < s.y && game[c.x + 1, c.y + 2] == t)
						{
							return new Move(c, MoveDirection.Right);
						}
					}
				}
			}
		}

		return default;
	}
}
