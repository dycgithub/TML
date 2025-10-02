using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivelyCamera : MonoBehaviour
{
    [SerializeField, Min(0f)] private float
        springStrength = 100f, //弹簧系数
        dampingStrength = 10f, //阻尼系数
        jostleStrength = 4f, //抖动强度   jostle-推挤
        pushStrength = 1f,
        maxDeltaTIme = 1f / 60f;
    

    private Vector3 anchorPosition,velocity;

    private void Awake()
    {
        anchorPosition=transform.localPosition;
    }

    public void JostleY() => velocity.y += jostleStrength;

    public void PushXZ(Vector2 impulse) //推动力
    {
        velocity.x += pushStrength * impulse.x;
        velocity.z += pushStrength * impulse.y;
    }

    void LateUpdate ()
    {
        float dt = Time.deltaTime;
        while (dt > maxDeltaTIme)
        {
            TimeStep(maxDeltaTIme);
            dt-=maxDeltaTIme;
        }
        TimeStep(dt);
    }

    private void TimeStep(float dt)
    {
        Vector3 displacement = anchorPosition - transform.localPosition;//位移
        Vector3 acceleration = springStrength * displacement - dampingStrength * velocity;//加速度=弹簧力-阻尼力
        velocity += acceleration * dt;
        transform.localPosition += velocity * dt;
    }
    
}
