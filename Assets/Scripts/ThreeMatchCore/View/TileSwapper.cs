using UnityEngine;

/// <summary>
/// View
///视觉效果处理
///TileSwapper 类专门负责处理 Tile 对象之间的交换动画效果
///通过控制 transform.localPosition 实现位置插值动画
///动画逻辑实现
///Swap 和 Update 方法处理瓦片交换的视觉动画过程
///使用 Mathf.Sin 和 Vector3.Lerp 实现平滑的交换过渡效果
/// </summary>


[System.Serializable]
public class TileSwapper
{
    [SerializeField, Range(0.1f, 10f)]
    float duration = 0.25f;

    [SerializeField, Range(0f, 1f)]
    float maxDepthOffset = 0.5f;

    Tile tileA, tileB;

    Vector3 positionA, positionB;

    float progress = -1f;

    bool pingPong;

    public float Swap (Tile a, Tile b, bool pingPong)
    {
        tileA = a;
        tileB = b;
        positionA = a.transform.localPosition;
        positionB = b.transform.localPosition;
        this.pingPong = pingPong;
        progress = 0f;
        return pingPong ? 2f * duration : duration;
    }

    public void Update ()
    {
        if (progress < 0f)
        {
            return;
        }

        progress += Time.deltaTime;
        if (progress >= duration)
        {
            if (pingPong)
            {
                progress -= duration;
                pingPong = false;
                (tileA, tileB) = (tileB, tileA);
            }
            else
            {
                progress = -1f;
                tileA.transform.localPosition = positionB;
                tileB.transform.localPosition = positionA;
                return;
            }
        }

        float t = progress / duration;
        float z = Mathf.Sin(Mathf.PI * t) * maxDepthOffset;
        Vector3 p = Vector3.Lerp(positionA, positionB, t);
        p.z = -z;
        tileA.transform.localPosition = p;
        p = Vector3.Lerp(positionA, positionB, 1f - t);
        p.z = z;
        tileB.transform.localPosition = p;
    }
}