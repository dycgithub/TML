using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedPointManager : AbaseUI
{
    public Button rootBtn;
    public Button btn1;
    public Button bag1;
    public Button shop1;
    public Button home1;
    public Button btn2;
    public Button bag2; 
    public Button shop2;
    public Button home2;
    private string[] strs = {Data.play, Data.one, Data.home};
    private void Awake()
    {
        //红点的设置和触发
        //红点回调的添加 --一般是刷新UI的
        RedSystem.Instance.Init();
        RedSystem.Instance.AddListener((num) => { SetUIRedPoint(btn1.transform, num); },Data.play, Data.one);
        RedSystem.Instance.AddListener((num) => { SetUIRedPoint(bag1.transform, num); },Data.play, Data.one, Data.bag);
        RedSystem.Instance.AddListener((num) => { SetUIRedPoint(shop1.transform, num); },Data.play, Data.one, Data.shop);
        RedSystem.Instance.AddListener((num) => { SetUIRedPoint(home1.transform, num); },Data.play, Data.one, Data.home);
        RedSystem.Instance.AddListener((num) => { SetUIRedPoint(btn2.transform, num); },Data.play, Data.two);
        RedSystem.Instance.AddListener((num) => { SetUIRedPoint(rootBtn.transform, num); },Data.play);
        RedSystem.Instance.AddListener((num) => { SetUIRedPoint(rootBtn.transform, num); }, strs);
      
    }
    private void Start()
    {
        rootBtn.onClick.AddListener(rootBtnCallBack);
        btn1.onClick.AddListener(btn1CallBack);
        btn2.onClick.AddListener(btn2CallBack);
        bag1.onClick.AddListener(bag1CallBack);
        bag2.onClick.AddListener(bag2CallBack);
        shop1.onClick.AddListener(shop1CallBack);
        shop2.onClick.AddListener(shop2CallBack);
        home1.onClick.AddListener(home1CallBack);
        home2.onClick.AddListener(home2CallBack);
    }
 
    private void OnEnable()
    {
        RedSystem.Instance.AddNodeNum((num) => { SetUIRedPoint(home2.transform, num); },Data.play, Data.two, Data.home);
        RedSystem.Instance.AddNodeNum((num) => { SetUIRedPoint(shop2.transform, num); },Data.play, Data.two, Data.shop);
        RedSystem.Instance.AddNodeNum((num) => { SetUIRedPoint(bag2.transform, num); },Data.play, Data.two, Data.bag);
        RedSystem.Instance.AddNodeNum((num) => { SetUIRedPoint(home1.transform, num); },Data.play,Data.one, Data.home);
    }
 
    private void Update()
    {
         RedSystem.Instance.Update();
    }
    #region 按钮回调
    public void rootBtnCallBack()
    {
        RedSystem.Instance.DeleteNode(Data.play);
    }
    public void btn1CallBack()
    {
        RedSystem.Instance.DeleteNode(Data.play, Data.one);
    }
    public void bag1CallBack()
    {
        RedSystem.Instance.DeleteNode(Data.play, Data.one, Data.bag);
    }
    public void shop1CallBack()
    {
        RedSystem.Instance.DeleteNode(Data.play, Data.one, Data.shop);
    }
    public void home1CallBack()
    {
        RedSystem.Instance.DeleteNode(Data.play, Data.one, Data.home);
    }
    public void btn2CallBack()
    {
        RedSystem.Instance.DeleteNode(Data.play, Data.two);
    }
    public void bag2CallBack()
    {
        RedSystem.Instance.DeleteNode(Data.play, Data.two,Data.bag);
    }
    public void shop2CallBack()
    {
        RedSystem.Instance.DeleteNode(Data.play, Data.two,Data.shop);
    }
    public void home2CallBack()
    {
        RedSystem.Instance.DeleteNode(Data.play, Data.two,Data.home);
    }
    #endregion
}
