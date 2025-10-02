using UnityEngine;
/// <summary>
///View
/// 负责瓦片的显示、动画效果（消失、下落）等视觉表现
/// 处理瓦片在游戏场景中的具体呈现
/// </summary>
public class Tile : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)] private float disappearDuration = 0.25f;//消失持续时间

    PrefabInstancePool<Tile> pool;

    private float disappearProgress;

    [System.Serializable]
    struct FallingState//下落状态
    {
        public float fromY, toY, duration, progress;
    }

    private FallingState falling;//下落

    public Tile Spawn (Vector3 position)
    {
        Tile instance = pool.GetInstance(this);
        instance.pool = pool;
        instance.transform.localPosition = position;
        instance.transform.localScale = Vector3.one;
        instance.disappearProgress = -1f;
        instance.falling.progress = -1f;
        instance.enabled = false;
        return instance;
    }

    public void Despawn () => pool.Recycle(this);

    public float Disappear ()
    {
        disappearProgress = 0f;
        enabled = true;
        return disappearDuration;
    }

    public float Fall (float toY, float speed)
    {
        falling.fromY = transform.localPosition.y;
        falling.toY = toY;
        falling.duration = (falling.fromY - toY) / speed;
        falling.progress = 0f;
        enabled = true;
        return falling.duration;
    }

    void Update ()
    {
        if (disappearProgress >= 0f)
        {
            disappearProgress += Time.deltaTime;
            if (disappearProgress >= disappearDuration)
            {
                Despawn();
                return;
            }
            transform.localScale =
                Vector3.one * (1f - disappearProgress / disappearDuration);
        }

        if (falling.progress >= 0f)
        {
            Vector3 position = transform.localPosition;
            falling.progress += Time.deltaTime;
            if (falling.progress >= falling.duration)
            {
                falling.progress = -1f;
                position.y = falling.toY;
                enabled = disappearProgress >= 0f;
            }
            else
            {
                position.y = Mathf.Lerp(
                    falling.fromY, falling.toY, falling.progress / falling.duration
                );
            }
            transform.localPosition = position;
        }
    }
}