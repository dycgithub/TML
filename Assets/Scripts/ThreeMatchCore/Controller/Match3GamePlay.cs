using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using static Unity.Mathematics.math;
/// <summary>
/// Controller
/// 负责协调游戏逻辑、处理用户输入并管理游戏状态 ; 验证操作合法性，更新游戏状态
/// StartNewGame
/// TryMove
/// ProcessMatches
/// DropTiles
/// FillGrid
/// FindMatches
/// </summary>
public class Match3GamePlay : MonoBehaviour
{
	[SerializeField]
	int2 size = 8;

	Grid2D<TileState> grid;

	List<MatchPosition> matches;

	int scoreMultiplier;

	public TileState this[int2 c] => grid[c];

	public TileState this[int x, int y] => grid[x, y];

	public int2 Size => size;

	public List<int2> ClearedTileCoordinates
	{ get; private set; }

	public List<TileDrop> DroppedTiles
	{ get; private set; }

	public List<SingleScore> Scores
	{ get; private set; }

	public bool NeedsFilling
	{ get; private set; }

	public Move PossibleMove
	{ get; private set; }

	public int TotalScore
	{ get; private set; }

	public bool HasMatches => matches.Count > 0;

	public void StartNewGame ()
	{
		TotalScore = 0;
		if (grid.IsUndefined)
		{
			grid = new(size);
			matches = new();
			ClearedTileCoordinates = new();
			DroppedTiles = new();
			Scores = new();
		}
		do
		{
			FillGrid();
			PossibleMove = Move.FindMove(this);//寻找可移动位置
		}
		while (!PossibleMove.IsValid);
	}
    public bool TryMove (Move move)
	{
		scoreMultiplier = 1;
		grid.Swap(move.From, move.To);
		if (FindMatches())
		{
			return true;
		}
		grid.Swap(move.From, move.To);
		return false;
	}//移动时机的逻辑

	public void ProcessMatches ()//
	{
		ClearedTileCoordinates.Clear();
		Scores.Clear();

		for (int m = 0; m < matches.Count; m++)
		{
			MatchPosition match = matches[m];
			int2 step = match.isHorizontal ? int2(1, 0) : int2(0, 1);
			int2 c = match.coordinates;
			for (int i = 0; i < match.length; c += step, i++)
			{
				if (grid[c] != TileState.None)
				{
					grid[c] = TileState.None;
					ClearedTileCoordinates.Add(c);
				}
			}

			var score = new SingleScore
			{
				position = match.coordinates + (float2)step * (match.length - 1) * 0.5f,
				value = match.length * scoreMultiplier++
			};
			Scores.Add(score);
			TotalScore += score.value;
		}

		matches.Clear();
		NeedsFilling = true;
	}//消除过程

	public void DropTiles ()
	{
		DroppedTiles.Clear();

		for (int x = 0; x < size.x; x++)
		{
			int holeCount = 0;
			for (int y = 0; y < size.y; y++)
			{
				if (grid[x, y] == TileState.None)
				{
					holeCount += 1;
				}
				else if (holeCount > 0)
				{
					grid[x, y - holeCount] = grid[x, y];
					DroppedTiles.Add(new TileDrop(x, y - holeCount, holeCount));
				}
			}

			for (int h = 1; h <= holeCount; h++)
			{
				grid[x, size.y - h] = (TileState)Random.Range(1, 8);
				DroppedTiles.Add(new TileDrop(x, size.y - h, holeCount));
			}
		}

		NeedsFilling = false;
		if (!FindMatches())
		{
			PossibleMove = Move.FindMove(this);
		}
	}//滑落

	void FillGrid ()
	{
		for (int y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++)
			{
				TileState a = TileState.None, b = TileState.None;
				int potentialMatchCount = 0;
				if (x > 1)
				{
					a = grid[x - 1, y];
					if (a == grid[x - 2, y])
					{
						potentialMatchCount = 1;
					}
				}
				if (y > 1)
				{
					b = grid[x, y - 1];
					if (b == grid[x, y - 2])
					{
						potentialMatchCount += 1;
						if (potentialMatchCount == 1)
						{
							a = b;
						}
						else if (b < a)
						{
							(a, b) = (b, a);
						}
					}
				}

				TileState t = (TileState)Random.Range(1, 8 - potentialMatchCount);
				if (potentialMatchCount > 0 && t >= a)
				{
					t += 1;
				}
				if (potentialMatchCount == 2 && t >= b)
				{
					t += 1;
				}
				grid[x, y] = t;
			}
		}
	}//动画逻辑

	bool FindMatches ()//消除逻辑
	{
		matches.Clear();
//横向检测
		for (int y = 0; y < size.y; y++)
		{
			TileState start = grid[0, y];
			int length = 1;
			for (int x = 1; x < size.x; x++)
			{
				TileState t = grid[x, y];
				if (t == start)
				{
					length += 1;
				}
				else
				{
					if (length >= 3)
					{
						matches.Add(new MatchPosition(x - length, y, length, true));
					}
					start = t;
					length = 1;
				}
			}
			if (length >= 3)
			{
				matches.Add(new MatchPosition(size.x - length, y, length, true));
			}
		}
//纵向检测
		for (int x = 0; x < size.x; x++)
		{
			TileState start = grid[x, 0];
			int length = 1;
			for (int y = 1; y < size.y; y++)
			{
				TileState t = grid[x, y];
				if (t == start)
				{
					length += 1;
				}
				else
				{
					if (length >= 3)
					{
						matches.Add(new MatchPosition(x, y - length, length, false));
					}
					start = t;
					length = 1;
				}
			}
			if (length >= 3)
			{
				matches.Add(new MatchPosition(x, size.y - length, length, false));
			}
		}

		return HasMatches;
	}//消除逻辑
}