using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl
{
    public Dictionary<string, SceneBase> dictScene;
    private static SceneControl instance;
    public static SceneControl GetInstance()
    {
        if (instance == null)
        {
            return instance;
        }
        return instance;
    }
/// <summary>
/// 
/// </summary>
/// <param name="sceneName">场景名称</param>
/// <param name="sceneBase">目标场景</param>
    public void LoadScene(string sceneName,SceneBase sceneBase)
    {
        if (!dictScene.ContainsKey(sceneName))
        {
            dictScene.Add(sceneName, sceneBase);
            
        }
        if (dictScene.ContainsKey(SceneManager.GetActiveScene().name))//退出当前场景
        {
            dictScene[SceneManager.GetActiveScene().name].ExitScene();
        }
        else Debug.LogError("当前场景未注册");

        GameRoot.GetInstance().UI_Root.Pop(true);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        sceneBase.EnterScene();
    }
}
