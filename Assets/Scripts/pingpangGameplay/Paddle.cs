using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 球拍
/// </summary>
public class Paddle : MonoBehaviour
{
    [SerializeField, Min(0f)] private float
        Minextents = 4f,
        Maxextents = 4f,
        speed = 10f,
        maxTargetingBias = 0.75f;

    [SerializeField]private 
        bool isAI=false;//是否是AI控制
    [SerializeField] private
        TMP_Text scoreText;
    private int score;
    private float extents,targetingBias;

    private void Awake()
    {
        SetScore(0);
    }
    public void Move(float target,float arenaExtents)
    {
        Vector3 pos = transform.localPosition;
        pos.x=isAI?AdjustByAI(pos.x,target):AdjustByPlayer(pos.x);
        float limit=arenaExtents-extents;//球拍移动的范围
        pos.x = Mathf.Clamp(pos.x, -limit, limit);
        transform.localPosition = pos;
    }
    private float AdjustByAI (float x, float target)
    {
        target+=targetingBias*extents;
        if (x < target)
        {
            return Mathf.Min(x + speed * Time.deltaTime, target);
        }
        return Mathf.Max(x - speed * Time.deltaTime, target);
    }
    private float AdjustByPlayer(float x)
    {
        bool goRight = Input.GetKey(KeyCode.RightArrow);
        bool goLeft = Input.GetKey(KeyCode.LeftArrow);
        if (goRight && !goLeft)
        {
            return x+speed*Time.deltaTime;
        }
        else if(goLeft&&!goRight)
        {
            return x-speed*Time.deltaTime;
        }
        return x;
    }
    /// <summary>
    /// 检测球是否在球拍的范围内,并计算击球因子
    /// </summary>
    /// <param name="ballX"></param>
    /// <param name="ballExtents"></param>
    /// <param name="hitFactor"></param>
    /// <returns></returns>
    public bool HitBall (float ballX, float ballExtents,out float hitFactor)
    {
        ChangeTargetingBias();
        hitFactor =
            (ballX - transform.localPosition.x) / (extents + ballExtents);
        return -1f <= hitFactor && hitFactor <= 1f;
    }
    public void StartNewGame()
    {
        SetScore(0);
        ChangeTargetingBias();
    }
    public bool ScorePoint(int pointsToWin)
    {
        SetScore(score + 1, pointsToWin);
        return score>=pointsToWin;
    }
    void SetScore(int newScore,float pointsToWin=10f)
    {
        score = newScore;
        scoreText.SetText("{0}", newScore);
        SetExtents(Mathf.Lerp(Maxextents,Minextents, newScore / (pointsToWin - 1f)));
        print(newScore / (pointsToWin - 1f));
    }
    
    private void ChangeTargetingBias()=> 
        targetingBias=Random.Range(-maxTargetingBias,maxTargetingBias);
    private void SetExtents(float newExtents)
    {
        extents = newExtents;
        Vector3 s = transform.localScale;
        s.x = 2f * newExtents;
        transform.localScale = s;
    }
}
