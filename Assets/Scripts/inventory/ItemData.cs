
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 拖拽开始时的处理逻辑
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 拖拽过程中的处理逻辑
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 拖拽结束时的处理逻辑
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 鼠标按下时的处理逻辑
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 鼠标进入时的处理逻辑
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标退出时的处理逻辑
    }
}


