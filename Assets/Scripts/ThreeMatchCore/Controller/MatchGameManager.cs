
using UnityEngine;
/// <summary>
/// Controller
/// 接收原始鼠标输入
/// 调用 Match3Skin.EvaluateDrag 进行具体的拖拽处理
/// 这体现了Controller层协调Input和View的职责
/// </summary>
public class MatchGameManager : MonoBehaviour
{
    [SerializeField] private Match3Skin match3;
    [SerializeField] private bool automaticPlay;
    
    private Vector3 dragStart;
    private bool isDragging;
    private void Awake()
    {
        match3.StartNewGame();
    }

    private void Update()
    {
        if (match3.IsPlaying)
        {
            if (!match3.IsBusy)
            {
                HandleInput();
            }
            match3.DoWork();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            match3.StartNewGame();
        }
    }

    private void HandleInput()
    {
        if (automaticPlay)
        {
            match3.DoAutomaticMove();
        }
        else if (!isDragging && Input.GetMouseButtonDown(0))
        {
            dragStart = Input.mousePosition;
            isDragging = true;
        }
        else if (isDragging && Input.GetMouseButton(0))
        {
            isDragging = match3.EvaluateDrag(dragStart, Input.mousePosition);
        }
        else
        {
            isDragging = false;
        }
    }
    
    
}
