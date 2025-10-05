using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSystem
{
    #region MyRegion
 
    private static RedSystem instance = new RedSystem();
 
    public static RedSystem Instance
    {
        get { return instance; }
    }
    public void Init()
    {
        root = new TrieTreeNode("root");
        ditTreeNodes = new List<TrieTreeNode>();
    }
    #endregion
    private TrieTreeNode root;
    private List<TrieTreeNode> ditTreeNodes;
 
    ///
    private TrieTreeNode AddOrGetNode(params string[] path)
    {
        var tempNode = root;
        foreach (var pathName in path)
        {
            if (tempNode.GetChild(pathName) == null)
            {
                var tempChild = tempNode.AddChild(pathName);
                tempChild.parentNode = tempNode;
                Debug.Log("红点系统添加了新的" + pathName + "节点");
            }
 
            tempNode = tempNode.GetChild(pathName);
        }
 
        if (tempNode.deep == 0)
        {
            tempNode.deep = path.Length;
        }
 
        return tempNode;
    }
 
    public void AddListener(Action<int> action, params string[] path)
    {
        if (action != null)
        {
            AddOrGetNode(path).AddListener(action);
        }
    }
 
    public void AddPoint(int num = 1, params string[] path)
    {
        var node = AddOrGetNode(path);
        node.AddRedPointNum(num);
    }
 
    private TrieTreeNode GetNode(params string[] path)
    {
        var tempNode = root;
        foreach (var pathName in path)
        {
            if (tempNode.GetChild(pathName) == null)
            {
                tempNode.AddChild(pathName);
                Debug.Log("红点系统添加了新的" + pathName + "节点");
            }
            tempNode = tempNode.GetChild(pathName);
        }
 
        return tempNode;
    }
    public int GetNodePointNum(params string[] path)
    {
        var tempNode = GetNode(path);
        return tempNode.num;
    }
    //这种方式符合单一职责原则，即红点系统负责管理回调的有效性，节点负责执行回调的逻辑。
    public void AddNodeNum(Action<int> callBack=null, params string[] path)
    {
       
        if (callBack != null)
        {
            var tempNode = AddOrGetNode(path);
            tempNode.AddListener(callBack);
        }
        AddNodeNum(path);
    }
    public void AddNodeNum(params string[] path)
    {
        var tempNode = AddOrGetNode(path);
        tempNode.IncreaseNodeValue();
        //父节点一条路上也要刷新
        tempNode = tempNode.parentNode;
        while (tempNode.parentNode != null)
        {
            if (!ditTreeNodes.Contains(tempNode))
            {
                ditTreeNodes.Add(tempNode);
            }
 
            tempNode = tempNode.parentNode;
        }
    }
    public void DeleteNode(params string[] path)
    {
        var tempNode = AddOrGetNode(path);
        tempNode.DecreaseNodeValue();
        //父节点一条路上也要刷新
        tempNode = tempNode.parentNode;
        while (tempNode.parentNode != null)
        {
            if (!ditTreeNodes.Contains(tempNode))
            {
                ditTreeNodes.Add(tempNode);
            }
            tempNode = tempNode.parentNode;
        }
    }
    public void Update()
    {
        if (ditTreeNodes.Count <= 0)
        {
            return;
        }
        //先排序
        for (int i = 0; i < ditTreeNodes.Count; i++)
        {
            for (int j = i; j < ditTreeNodes.Count - 1; j++)
            {
                if (ditTreeNodes[j].deep < ditTreeNodes[j + 1].deep)
                {
                    (ditTreeNodes[j], ditTreeNodes[j + 1]) = (ditTreeNodes[j + 1], ditTreeNodes[j]);
                }
            }
        }
        foreach (var nodes in ditTreeNodes)
        {
            //检查自己的是否需要刷新值
            nodes.GetNodeValue();
        }
        ditTreeNodes.Clear();
    }
    
     

}
