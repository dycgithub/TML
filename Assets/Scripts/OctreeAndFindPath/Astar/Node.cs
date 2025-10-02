using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 八叉树A*寻路的节点
/// </summary>
public class Node
{
    public OctreeNode octreeNode;
    public List<Edge> edgeList = new();//与其他节点的连接边
    
    //A*
    public float g, h;
    public float f=>g+h;//A星的总代价
    public Node cameFrom;
    public Node(OctreeNode octreeNode)
    {
        this.octreeNode = octreeNode;
    }
}
