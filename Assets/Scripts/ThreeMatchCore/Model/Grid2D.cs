using Unity.Mathematics;
/// <summary>
///Model，负责数据的存储、访问和基本操作
///Grid2D结构体负责存储和管理二维网格数据
///通过cells 数组存储实际数据，这是典型的数据模型
/// </summary>
[System.Serializable]
public struct Grid2D<T>
{
    T[] cells;

    int2 size;

    public T this[int x, int y]
    {
        get => cells[y * size.x + x];
        set => cells[y * size.x + x] = value;
    }

    public T this[int2 c]
    {
        get => cells[c.y * size.x + c.x];
        set => cells[c.y * size.x + c.x] = value;
    }

    public int2 Size => size;

    public int SizeX => size.x;

    public int SizeY => size.y;

    public bool IsUndefined => cells == null || cells.Length == 0;

    public Grid2D (int2 size)
    {
        this.size = size;
        cells = new T[size.x * size.y];
    }

    public bool AreValidCoordinates (int2 c) =>
        0 <= c.x && c.x < size.x && 0 <= c.y && c.y < size.y;

    public void Swap (int2 a, int2 b) => (this[a], this[b]) = (this[b], this[a]);
    
}