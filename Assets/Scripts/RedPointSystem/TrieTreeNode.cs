using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///用params来输入键用string会GC
///避免重复父类触发刷新
///当子节点添加数量时候把父节点全部标记为脏节点
///添加时候判断是否已经在列表中,这样多个字节点在一帧内改变只会更新父节点一次值
/// 里面把我们的节点更具深度排序后更新,因为只更新一次,必须先更新深度深度深的节点,这样他的父节点才能获取到正确的值,否则会出现节点数据的错误
///将节点刷新显示的方法统一,减少红点系统使用的复杂度
/// </summary>
public class TrieTreeNode 
{
    public string name;
    public int num;
    public Dictionary<string, TrieTreeNode> childDic=new Dictionary<string, TrieTreeNode>() ;
    public Action<int> onValueChange;
    public TrieTreeNode parentNode;
    public int deep = 0;
    public TrieTreeNode GetChild(string name)
    {
        if (!childDic.ContainsKey(name))
        {
            return null;
        }
        return childDic[name];
    }
    public TrieTreeNode AddChild(string name)
    {
        if (!childDic.ContainsKey(name))
        {
            var tempNode = new TrieTreeNode(name);
            childDic.Add(name, tempNode);
            childDic[name].num += num;
            return tempNode;
        }
        return childDic[name];
    }
    public void AddRedPointNum(int num=1)
    {
        this.name += name;
    }
    public TrieTreeNode(string mName)
    {
        name = mName;
    }
 
    public void AddListener(Action<int> action)
    {
        onValueChange+= action;
        action.Invoke(num);
    }
    public void RemoveListener(Action<int> action)
    {
        onValueChange-= action;
        action.Invoke(num);
    }
    public void IncreaseNodeValue()
    {
        if (childDic.Count > 0)
        {
            Debug.LogError("只能在子节点添加红点");
            return;
        }
        num++;
        onValueChange?.Invoke(num);
    }
    public void DecreaseNodeValue()
    {
        if (childDic.Count > 0)
        {
            Debug.LogError("只能在子节点直接删除红点");
            return;
        }
        if (num > 0)
        {
            num--;
            onValueChange?.Invoke(num);
        }
    }
    //这个不是修改 这个是更新
    public void GetNodeValue()
    {
        var currentNum=0;
        foreach (var child in childDic.Values)
        {
            currentNum += child.num;
        }
        if(num!=currentNum)
        {
            num = currentNum;
            onValueChange?.Invoke(num);
        }
    }

}
