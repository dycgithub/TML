using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private UIManager uiManager;
    public UIManager UI_Root
    {
        get => uiManager;
    }

    private SceneControl sceneControl;
    public SceneControl Scene_Root
    {
        get => sceneControl;
    }
    
    public static GameRoot instance;

    public static GameRoot GetInstance()
    {
        if(instance==null)
        {
            return instance;
        }
        return instance;
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        instance = this;
        uiManager = new UIManager();
        sceneControl = new SceneControl();
        
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        UI_Root.canvas = UIMethods.GetInstance().FindCanvas();
        
        //UI_Root.Push(new StartPanel());


        UI_Root.Push(new ThridPanel());
        UI_Root.Push(new FirstPanel());
    }
    
    //push的时候要有实例化的对象,通过继承baseUI父类来实例化resource里的预制体
    
}
