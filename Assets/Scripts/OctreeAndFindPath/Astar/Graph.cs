
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public List<Node> nodeList = new List<Node>();
    public List<Edge> edgeList = new List<Edge>();

    private Ray m_CacheRay = new();//缓存射线
    private List<Vector3> m_SixDirs = new()
    {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right,
        Vector3.up,
        Vector3.down
    };

    public void AddNode(OctreeNode octreeNode)//将八叉树节点加入图节点列表
    {
        if (FindNode(octreeNode.id) == null)
        {
            Node node = new Node(octreeNode);
            nodeList.Add(node);
        }
    }

    public Node FindNode(int octreeId)//查找图中与八叉树对应的节点
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].octreeNode.id == octreeId)
            {
                return nodeList[i];
            }
        }
        return null;
    }

    public void AddEdge(OctreeNode fromOctreeNode, OctreeNode toOctreeNode)
    {
        if (FindEdge(fromOctreeNode, toOctreeNode) != null)
        {
            return;
        }

        Node from = FindNode(fromOctreeNode.id);//查找图中与八叉树对应的节点
        Node to = FindNode(toOctreeNode.id);//查找图中与八叉树对应的节点
        
        if(from!=null&&to!=null)//创建双向边界
        {
            Edge edge = new Edge(from, to);
            edgeList.Add(edge);
            from.edgeList.Add(edge);
            
            Edge reserveEdge = new Edge(to, from);
            edgeList.Add(reserveEdge);
            to.edgeList.Add(reserveEdge);
        }
        
    }//将查找到的八叉树节点关系加入图的边界列表
    public Edge FindEdge(OctreeNode fromOctreeNode, OctreeNode toOctreeNode)//查找图中与八叉树对应的边
    {
        Node from = FindNode(fromOctreeNode.id);
        Node to = FindNode(toOctreeNode.id);
        if(from!=null&&to!=null)
        {
            for (int i = 0; i < from.edgeList.Count; i++)
            {
                var element=from.edgeList[i];
                if (element.endNode.octreeNode.id == toOctreeNode.id)
                {
                    return element;
                }
            }
        }
        return null;
    }

    public void DrawDebug()
    {
        foreach (var node in nodeList)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(node.octreeNode.bounds.center, 0.25f);
        }

        for (int i = 0; i < edgeList.Count; i++)
        {
            Debug.DrawLine(edgeList[i].startNode.octreeNode.bounds.center, edgeList[i].endNode.octreeNode.bounds.center, Color.red);
        }
    }

    public void ConnectNodeNeighboursNode()//射线连接节点与邻居
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            for (int j = 0; j < nodeList.Count; j++)
            {
                if (i == j)
                {
                    continue;                    
                }

                for (int k = 0; k < m_SixDirs.Count; k++)
                {
                    m_CacheRay.origin = nodeList[i].octreeNode.bounds.center;
                    m_CacheRay.direction= m_SixDirs[k];
                    float maxLength = nodeList[i].octreeNode.bounds.size.x / 2.0f + 1.0f;
                    if (nodeList[j].octreeNode.bounds.IntersectRay(m_CacheRay, out float hitLength))
                    {
                        if (hitLength <= maxLength)
                        {
                            AddEdge(nodeList[i].octreeNode, nodeList[j].octreeNode);
                        }
                    }
                }
            }
        }
    }

    public bool AStar(OctreeNode startNode, OctreeNode endNode, ref List<Node> pathList)
    {
       Node start = FindNode(startNode.id);
       Node end = FindNode(endNode.id);
       if (start == null || end == null)
       {
           return false;
       }
       // 一开始就是终点
       if (start.octreeNode.id == end.octreeNode.id)
       {
           end.cameFrom = start;
           ReconstructPath(start, end, ref pathList);
           return true;
       }
       List<Node>openLst=new List<Node>();//对比权重的列表
       List<Node>closeLst=new List<Node>();//走过的路径
       //初始权重
       start.g = 0;
       //启发函数-欧几里得距离
       start.h= Vector3.SqrMagnitude(endNode.bounds.center-startNode.bounds.center);
        
       openLst.Add(start);
       while (openLst.Count > 0)
       {
           int thisIndex = GetLowestFIndex(openLst);
           Node thisNode = openLst[thisIndex];

           if (thisNode.octreeNode.id == endNode.id)//找到终点
           {
               ReconstructPath(start, end, ref pathList);
               return true;
           }
           
           openLst.RemoveAt(thisIndex);//从比对节点队列移除当前节点
           closeLst.Add(thisNode);//加入已经比对过的节点
            //找这个节点代价最小的邻居节点
           foreach (Edge edge in thisNode.edgeList)//遍历所有边
           {
               Node edgeEndNode= edge.endNode;//边的终点
               if (closeLst.IndexOf(edgeEndNode) > -1)//已经走过的节点
               {
                   continue;
               }
               bool updateNode = false;//是否更新节点
               float newG = thisNode.g + Vector3.SqrMagnitude(edgeEndNode.octreeNode.bounds.center - thisNode.octreeNode.bounds.center);
               //首次被发现
               if (openLst.IndexOf(edgeEndNode) <= -1)
               {
                   openLst.Add(edgeEndNode);
                   updateNode = true;
               }
               //有更近的入口
               else if(newG<=edgeEndNode.g)
               {
                   updateNode = true;
               }

               if (updateNode)
               {
                   edgeEndNode.cameFrom = thisNode;//更新路径
                   edgeEndNode.g = newG;//更新g值
                   //启发函数-欧几里得距离
                   edgeEndNode.h=Vector3.SqrMagnitude(endNode.bounds.center-edgeEndNode.octreeNode.bounds.center);
               }
           }
           
       }
       return false;
    }

    private int GetLowestFIndex(List<Node> openList)//找到最短路径
    {
        float LowestFScore = -9999f;
        int lowestIndex = 0;
        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i].f <= LowestFScore)
            {
                LowestFScore = openList[i].f;
                lowestIndex = i;
            }
        }
        return lowestIndex;
    }

    private void ReconstructPath(Node startNode, Node endNode, ref List<Node> pathList)
    {//找到后回溯路径存储起来
        pathList.Clear();
        pathList.Add(endNode);
        var from= endNode.cameFrom;//从终点开始回溯
        while (from != null && from != startNode)
        {
            pathList.Insert(0, from);//插入路径
            from = from.cameFrom;
        }
        pathList.Insert(0, startNode);//插入起点
    }
    
}
