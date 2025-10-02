using TMPro;
using UnityEngine;
/// <summary>
/// View不直接处理业务逻辑，只负责呈现结果
/// 浮动分数效果
/// 处理动画效果（上升、淡出）
///管理显示时长和移动速度
///这些都属于UI/视觉效果的范畴
///数据流向
///通过 Show 方法接收位置和数值参数进行显示
/// </summary>
public class FloatingScore : MonoBehaviour
{
    [SerializeField]private TMP_Text displayText;//播放文本

    [SerializeField, Range(0.1f, 1f)]
    private float displayDuration = 0.5f;//显示持续时间

    [SerializeField, Range(0f, 4f)]
    private float riseSpeed = 2f;//上升速度

    private float age;//当前存在时间

    PrefabInstancePool<FloatingScore> pool;

    public void Show (Vector3 position, int value)
    {
        FloatingScore instance = pool.GetInstance(this);
        instance.pool = pool;
        instance.displayText.SetText("{0}", value);
        instance.transform.localPosition = position;
        instance.age = 0f;
    }

    void Update ()
    {
        age += Time.deltaTime;
        if (age >= displayDuration)
        {
            pool.Recycle(this);
        }
        else
        {
            Vector3 p = transform.localPosition;
            p.y += riseSpeed * Time.deltaTime;
            transform.localPosition = p;
        }
    }
}