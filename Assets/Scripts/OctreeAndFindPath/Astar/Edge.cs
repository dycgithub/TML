/// <summary>
/// A*寻路 的边
/// 边
/// </summary>
public class Edge
{
    public Node startNode;
    public Node endNode;

    public Edge(Node from, Node to)
    {
        startNode = from;
        endNode = to;
    }
}
