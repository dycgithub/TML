using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// 球
/// </summary>
public class Ball : MonoBehaviour
{
   [SerializeField, Min(0f)] private //限制值最小为零
      float
      maxXSpeed = 20f,//球X轴的最大速度
      startXspeed = 2f,//球的范围
      constantYSpeed = 10f,//球的范围
      extents = 0.5f;//球的范围

   private Vector2 position, velocity;//位置和速度
   public float Extents=>extents;//球的范围
   public Vector2 Position=>position;//球的位置
   public Vector2 Velocity=>velocity;//球的速度

   private void Awake()
   {
      gameObject.SetActive(false);
   }

   /// <summary>
   /// 更新球的位置
   /// </summary>
   public void UpdateVisualization() => transform.localPosition = new Vector3(position.x, 0, position.y);
   /// <summary>
   /// 球的移动,每秒移动constantXspeed和constantYspeed个单位
   /// 外部接口
   /// </summary>
   /// <returns></returns>
   public void Move()=>position+=velocity*Time.deltaTime;
   /// <summary>
   /// 开始游戏
   /// </summary>
   public void StartNewGame()
   {
      position = Vector2.zero;
      UpdateVisualization();
      velocity.x = Random.Range(-startXspeed, startXspeed);
      velocity.y = -constantYSpeed;
      gameObject.SetActive(true);
   }
   /// <summary>
   /// 边界反弹
   /// </summary>
   /// <param name="boundary"></param>
   public void BounceX (float boundary)
   {
      position.x = 2f * boundary - position.x;
      velocity.x = -velocity.x;
   }
   public void BounceY (float boundary)
   {
      position.y = 2f * boundary - position.y;
      velocity.y = -velocity.y;
   }
   public void SetXPositionAndSpeed(float start,float speedFactor,float deltaTime)
   {
      velocity.x = maxXSpeed * speedFactor;
      position.x = start + velocity.x * deltaTime;
   }

   public void EndGame()
   {
      position.x = 0f;
      gameObject.SetActive(false);
   }
   
}
