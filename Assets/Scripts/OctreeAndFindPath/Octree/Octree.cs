

using System.Collections.Generic;
using UnityEngine;

public class Octree
{
    public OctreeNode rootNode;
    public List<OctreeNode> emptyLeaves = new();//空的叶子节点
    public Graph navigationGraph;

    public Octree(GameObject[] worldObjects, float minNodeSize, Graph navGraph)
    {
        navigationGraph = navGraph;//导航图

        Bounds bounds = new();//八叉树的边界
        foreach (var wObj in worldObjects)
        {
            bounds.Encapsulate(wObj.GetComponent<Collider>().bounds);//包含物体的边界
        }
        float maxSize=Mathf.Max(bounds.size.x,bounds.size.y,bounds.size.z);//最大的边界尺寸
        Vector3 sizeVector = new Vector3(maxSize, maxSize, maxSize) * 1.0f;//创建一个立方体边界
        bounds.SetMinMax(bounds.center - sizeVector, bounds.center + sizeVector);//设置边界的最小和最大值，使其成为一个立方体
        rootNode = new OctreeNode(bounds, minNodeSize, null);//创建根节点
        
        AddWorldObject(worldObjects);
        InitEmptyLeaves(rootNode);           
        navigationGraph.ConnectNodeNeighboursNode();
    }

    public void AddWorldObject(GameObject[] worldObjects)//添加世界物体
    {
        foreach (var go in worldObjects)
        {
            rootNode.DivideAndAdd(go);
        }
    }

    public void InitEmptyLeaves(OctreeNode octreeNode)//初始化空的叶子节点
    {
        emptyLeaves.Clear();
        if (octreeNode == null)
        {
            return;
        }

        if (octreeNode.children == null)//根节点记录
        {
            if (octreeNode.containedObjects.Count <= 0)
            {
                emptyLeaves.Add(octreeNode);
                navigationGraph.AddNode(octreeNode);
            }
        }
        else//子节点递归查询
        {
            for (int i = 0; i < octreeNode.children.Length; i++)
            {
                InitEmptyLeaves(octreeNode.children[i]);
            }
        }
    }

    public int FindEmptyLeafNode(OctreeNode node, Vector3 position)//查找包含位置的空叶子节点
    {
        int found = -1;
        if (node == null) return -1;
        if (node.children == null || node.children.Length <= 0)//叶子节点
        {
            if (node.bounds.Contains(position) && node.containedObjects.Count <= 0)
            {
                return node.id;
            }
        }
        else
        {
            for (int i = 0; i < node.children.Length; i++)//遍历子节点
            {
                found = FindEmptyLeafNode(node.children[i], position);//递归查找
                if (found != -1)
                {
                    break;          
                }
            }
        }
        return found;
    }
    public void DrawDebug()
    {
        
    }
}
