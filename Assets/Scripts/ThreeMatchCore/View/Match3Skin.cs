using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using static Unity.Mathematics.math;//直接使用math的方法不用类名
/// <summary>
/// View: 接收原始输入（鼠标、触摸），转换为有意义的操作
/// View层关注：如何从屏幕输入识别用户意图Controller层关注：移动操作是否合法及游戏状态更新
///StartNewGame
///DoWork
///DoAutomaticMove
///EvaluateDrag
///DropTiles
///ProcessMatches
/// DoMove
///ScreenToTileSpace
///SpawnTile
/// </summary>
public class Match3Skin : MonoBehaviour
{
    [SerializeField] private Tile[] tilePrefabs;//
    [SerializeField] private FloatingScore floatingScorePrefab;//浮点分数
    [SerializeField] private TMP_Text gameOverText, totalScoreText;//游戏结束文本，总分文本
    [SerializeField]private Match3GamePlay gamePlay;//游戏管理器
    
    [SerializeField,Range(0.1f,1f)] private float dragThreshold = 0.5f;//拖动阈值 
    [SerializeField, Range(0.1f, 20f)] private float dropSpeed = 8f;//掉落速度
    [SerializeField(),Range(0f,10f)] private float newDropOffset = 2f;//新掉落偏移
        
    [SerializeField()] private TileSwapper tileSwapper;//交换器
    
    private float busyDuration;//忙碌持续时间

    private Grid2D<Tile> tiles;//瓦片网格

    private float2 tileOffset;//瓦片偏移

    private float floatingScoreZ;//浮动分数Z轴
    
    public bool IsBusy => busyDuration > 0f;//是否忙碌
    public bool IsPlaying => IsBusy || gamePlay.PossibleMove.IsValid;//是否在玩

    public void StartNewGame () {
		busyDuration = 0f;
		totalScoreText.SetText("0");
		gameOverText.gameObject.SetActive(false);
		
		gamePlay.StartNewGame();
		tileOffset = -0.5f * (float2)(gamePlay.Size - 1);
		if (tiles.IsUndefined)
		{
			tiles = new(gamePlay.Size);
		}
		else
		{
			for (int y = 0; y < tiles.SizeY; y++)
			{
				for (int x = 0; x < tiles.SizeX; x++)
				{
					tiles[x, y].Despawn();
					tiles[x, y] = null;
				}
			}
		}

		for (int y = 0; y < tiles.SizeY; y++)
		{
			for (int x = 0; x < tiles.SizeX; x++)
			{
				tiles[x, y] = SpawnTile(gamePlay[x, y], x, y);
			}
		}
	}

	public void DoWork()
	{
		if (busyDuration > 0f)
		{
			tileSwapper.Update();
			busyDuration -= Time.deltaTime;
			if (busyDuration > 0f)
			{
				return;
			}
		}

		if (gamePlay.HasMatches)
		{
			ProcessMatches();
		}
		else if (gamePlay.NeedsFilling)
		{
			DropTiles();
		}
		else if (!IsPlaying)
		{
			gameOverText.gameObject.SetActive(true);
		}
	}

	public void DoAutomaticMove () => DoMove(gamePlay.PossibleMove);

	public bool EvaluateDrag (Vector3 start, Vector3 end)
	{
		float2 a = ScreenToTileSpace(start), b = ScreenToTileSpace(end);
		var move = new Move(
			(int2)floor(a), (b - a) switch
			{
				var d when d.x > dragThreshold => MoveDirection.Right,
				var d when d.x < -dragThreshold => MoveDirection.Left,
				var d when d.y > dragThreshold => MoveDirection.Up,
				var d when d.y < -dragThreshold => MoveDirection.Down,
				_ => MoveDirection.None
			}
		);
		if (
			move.IsValid &&
			tiles.AreValidCoordinates(move.From) && tiles.AreValidCoordinates(move.To)
			)
		{
			DoMove(move);
			return false;
		}
		return true;
	}

	void DropTiles ()
	{
		gamePlay.DropTiles();
		
		for (int i = 0; i < gamePlay.DroppedTiles.Count; i++)
		{
			TileDrop drop = gamePlay.DroppedTiles[i];
			Tile tile;
			if (drop.fromY < tiles.SizeY)
			{
				tile = tiles[drop.coordinates.x, drop.fromY];
			}
			else
			{
				tile = SpawnTile(
					gamePlay[drop.coordinates], drop.coordinates.x, drop.fromY + newDropOffset
				);
			}
			tiles[drop.coordinates] = tile;
			busyDuration = Mathf.Max(
				tile.Fall(drop.coordinates.y + tileOffset.y, dropSpeed), busyDuration
			);
		}
	}

	void ProcessMatches ()
	{
		gamePlay.ProcessMatches();

		for (int i = 0; i < gamePlay.ClearedTileCoordinates.Count; i++)
		{
			int2 c = gamePlay.ClearedTileCoordinates[i];
			busyDuration = Mathf.Max(tiles[c].Disappear(), busyDuration);
			tiles[c] = null;
		}

		totalScoreText.SetText("{0}", gamePlay.TotalScore);

		for (int i = 0; i < gamePlay.Scores.Count; i++)
		{
			SingleScore score = gamePlay.Scores[i];
			floatingScorePrefab.Show(new Vector3(score.position.x + tileOffset.x, score.position.y + tileOffset.y, floatingScoreZ), score.value);
			floatingScoreZ = floatingScoreZ <= -0.02f ? 0f : floatingScoreZ - 0.001f;
		}
	}

	void DoMove (Move move)
	{
		bool succcess = gamePlay.TryMove(move);//尝试移动
		Tile a = tiles[move.From], b = tiles[move.To];
		busyDuration = tileSwapper.Swap(a, b, !succcess);//交换
		if (succcess)
		{
			tiles[move.From] = b;
			tiles[move.To] = a;
		}
	}

	float2 ScreenToTileSpace (Vector3 screenPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		Vector3 p = ray.origin - ray.direction * (ray.origin.z / ray.direction.z);
		return float2(p.x - tileOffset.x + 0.5f, p.y - tileOffset.y + 0.5f);
	}

	Tile SpawnTile (TileState t, float x, float y) =>
		tilePrefabs[(int)t - 1].Spawn(new Vector3(x + tileOffset.x, y + tileOffset.y));
}