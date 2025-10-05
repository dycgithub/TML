using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// View
/// </summary>

public class RewadEffect : MonoBehaviour
{
    PrefabInstancePool<RewadEffect> pool;
    
    private float age;//已存活时间
    private float moveSpeed = 3f;//速度
    private float displayDuration = 3f;//可存活时间
    private Vector3 target=Vector3.zero;
    
    public void Show (Vector3 spawnPosition,Vector3 targetScorePosition)
    {
        RewadEffect instance = pool.GetInstance(this);
        instance.pool = pool;
        instance.transform.localPosition = spawnPosition;
        instance.age = 0f; // 重置存活时间
    
        // 添加随机扰动
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.5f, 0.5f),
            Random.Range(-0.5f, 0.5f),
            0f
        );
    
        Vector3 disturbedPosition = spawnPosition + randomOffset;
    
        // 先移动到扰动位置，再移动到目标位置
        instance.transform.DOLocalMove(disturbedPosition, displayDuration * 0.2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                instance.transform.DOLocalMove(targetScorePosition, displayDuration * 0.8f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() => {
                        instance.pool.Recycle(instance);
                    });
            });
    }

    void Update ()
    {
        age += Time.deltaTime;
        
    }
}
