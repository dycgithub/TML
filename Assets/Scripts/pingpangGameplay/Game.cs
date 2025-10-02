using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 球与球拍的通信
/// 游戏文字管理
/// </summary>
public class Game : MonoBehaviour
{
    [SerializeField]private
    Ball ball;

    [SerializeField]private
    Paddle bottomPaddle, topPaddle;

    [SerializeField, Min(0f)] private
        Vector2 arenaExtents = new Vector2(10f, 10f);//球移动的范围
    
    [SerializeField,Min(2)]private
        int pointsToWin=5;//赢得比赛的分数

    [SerializeField] private
        TMP_Text countDownText;
    
    [SerializeField,Min(1f)] private
        float newGameDelay=3f;

    private float countDownUntilNewGame;
    
    [SerializeField]
    LivelyCamera livelyCamera;
    
    private void Awake()=>countDownUntilNewGame=newGameDelay;
    
    private void Update()
    {
        bottomPaddle.Move(ball.Position.x, arenaExtents.x);
        topPaddle.Move(ball.Position.x, arenaExtents.x);
        if (countDownUntilNewGame <= 0f)
        {
            UpdateGame();
        }
        else
        {
            UpdateCountdown();
        }
    }

    private void UpdateGame()
    {
        ball.Move();
        BounceYIfNeeded();
        BounceXIfNeeded(ball.Position.x);
        ball.UpdateVisualization();
    }
    
    void UpdateCountdown ()
    {
        countDownUntilNewGame -= Time.deltaTime;
        if (countDownUntilNewGame <= 0f)
        {
            //优化countDownText.gameObject.transform.localPosition = new Vector3(0, 1000, 0);
            countDownText.gameObject.SetActive(false);
            StartNewGame();
        }
        else
        {
            float displayValue = Mathf.Ceil(countDownUntilNewGame);
            if (displayValue < newGameDelay)
            {
                countDownText.SetText("{0}", displayValue);
            }
        }
        countDownText.SetText("{0}", countDownUntilNewGame);
    }
    
    private void StartNewGame()
    {
        ball.StartNewGame();
        bottomPaddle.StartNewGame();
        topPaddle.StartNewGame();
    }
    
    private void BounceXIfNeeded(float x)
    {
        float xExtents=arenaExtents.x-ball.Extents;//球的x轴范围
        if (x < -xExtents)
        {
            livelyCamera.PushXZ(ball.Velocity);
            ball.BounceX(-xExtents);
        }
        else if(x>xExtents)
        {
            livelyCamera.PushXZ(ball.Velocity);
            ball.BounceX(xExtents);
        }
    }

    private void BounceYIfNeeded()//检测球是否需要在y轴反弹实际是Z轴
    {
        float yExtents=arenaExtents.y-ball.Extents;//球的y轴范围
        if (ball.Position.y < -yExtents)
        {
            BounceY(-yExtents, bottomPaddle,topPaddle);
        }
        else if (ball.Position.y > yExtents)
        {
            BounceY(yExtents, topPaddle,bottomPaddle);
        }
    }

    void BounceY(float boundary, Paddle defender,Paddle attacker)
    {
        float durationAfterBounce=(ball.Position.y-boundary)/ball.Velocity.y;//计算球反弹后还需要移动的时间
        float bounceX=ball.Position.x-ball.Velocity.x*durationAfterBounce;
        BounceXIfNeeded(bounceX);
        bounceX=ball.Position.x-ball.Velocity.x*durationAfterBounce;
        
        livelyCamera.PushXZ(ball.Velocity);
        ball.BounceY(boundary);
        if (defender.HitBall(bounceX, ball.Extents, out float hitFactor))
        {
            ball.SetXPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
        }
        else
        {
            livelyCamera.JostleY();
            if (attacker.ScorePoint(pointsToWin))
            {
                EndGame();
            }
        }
    }

    private void EndGame()
    {
        countDownUntilNewGame = newGameDelay;
        countDownText.SetText("GAME OVER");
        countDownText.gameObject.SetActive(true);
        ball.EndGame();
    }
}
