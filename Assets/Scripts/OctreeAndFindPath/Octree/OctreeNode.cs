using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 八叉树的节点数据结构
/// Represents an axis aligned bounding box.
///表示轴对齐的边界框。
///An axis-aligned bounding box, or AABB for short, is a box aligned with coordinate axes and fully enclosing some object. Because the box is never rotated with respect to the axes, it can be defined by just its center and extents, or alternatively by min and max points.
///轴对齐边界框（简称 AABB）是与坐标轴对齐并完全包围的框 一些对象。因为盒子永远不会相对于轴旋转，所以它可以仅由其 中心和范围 ，或者按最小点和最大点。
/// </summary>

public class OctreeObject//八叉树对象
{ 
    public Bounds bounds;
    public GameObject go;

    public OctreeObject(GameObject gameObject)
    {
        go = gameObject;
        bounds = go.GetComponent<Collider>().bounds;
    }
}
public class OctreeNode//八叉树节点
{
    //此节点的信息
    public int id;
    public float minSize;
    public Bounds bounds=new();
    public Bounds[] childBounds = null;
    //此节点的父节点
    public OctreeNode parent;
    //子节点信息
    public bool isContainedChild = false;
    public OctreeNode[] children = null;
    //此节点包含的物体
    public List<OctreeObject>containedObjects=new();

    public OctreeNode(Bounds bounds, float minSize, OctreeNode parent)//构造函数
    {
        this.id = Utils.idInt++;//确保每个节点id不同
        this.parent= parent;
        this.bounds = bounds;
        this.minSize = minSize;
        BuildChildBounds();
    }

    private void BuildChildBounds()//创建八个子节点
    {
        float quarter = bounds.size.x / 4f;//四分之一
        Vector3 childSize=new Vector3(bounds.size.x/2f,bounds.size.x/2f,bounds.size.x/2f);
        childBounds = new[]
        {
            // 4 2 1
            new Bounds(bounds.center + new Vector3(-quarter, -quarter, -quarter), childSize), // 0
            new Bounds(bounds.center + new Vector3(-quarter, -quarter, quarter), childSize), // 1    
            new Bounds(bounds.center + new Vector3(-quarter, quarter, -quarter), childSize), // 2
            new Bounds(bounds.center + new Vector3(-quarter, quarter, quarter), childSize), // 3
            new Bounds(bounds.center + new Vector3(quarter, -quarter, -quarter), childSize), // 4
            new Bounds(bounds.center + new Vector3(quarter, -quarter, quarter), childSize), // 5
            new Bounds(bounds.center + new Vector3(quarter, quarter, -quarter), childSize), // 6
            new Bounds(bounds.center + new Vector3(quarter, quarter, quarter), childSize), // 7
        };
    }

    public void DivideAndAdd(GameObject worldObject)//划分节点并添加物体
    {
        OctreeObject octreeObject = new OctreeObject(worldObject);//创建八叉树物体
        //添加
        if (bounds.size.x <= minSize)//如果节点小于最小尺寸，直接添加
        {
            octreeObject.go.name = $"octreenode_go_in_{id}";//重命名
            containedObjects.Add(octreeObject);//添加物体
            return;
        }
        //划分
        if(children==null)children=new OctreeNode[8];
        for (int i = 0; i < 8; i++)
        {
            if (children[i] == null) children[i] = new OctreeNode(childBounds[i], minSize, this);
            if (children[i].bounds.Intersects(octreeObject.bounds))
            {
                isContainedChild = true;
                children[i].DivideAndAdd(worldObject);//递归划分
            }
        }

        if (!isContainedChild)
        {
            children = null;
        }
        
    }

    public void DrawDebug()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        if (containedObjects.Count > 0)
        {
            Gizmos.color = new Color(0, 0, 1, 0.75f);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
            foreach (var obj in containedObjects)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(obj.bounds.center,obj.bounds.size);
            }
        }
        //递归绘制子节点
        if (children != null)
        {
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] != null)
                {
                    children[i].DrawDebug();
                }
            }
        }
    }
    
}
