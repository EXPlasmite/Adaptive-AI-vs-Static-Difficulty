using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class PathfindingGrid : MonoBehaviour
{
    public static PathfindingGrid Instance;

    public Tilemap wallTilemap;
    public Vector2 gridWorldSize;
    public float nodeRadius = 0.5f;

    Node[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        Instance = this;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        Debug.Log("Grid created: " + gridSizeX + "x" + gridSizeY);
    }

    void CreateGrid()
    {
        int unwalkableCount = 0;
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - 
            Vector3.right * gridWorldSize.x / 2 - 
            Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + 
                    Vector3.right * (x * nodeDiameter + nodeRadius) + 
                    Vector3.up * (y * nodeDiameter + nodeRadius);
                
                Vector3Int cellPos = wallTilemap.WorldToCell(worldPoint);
                bool walkable = wallTilemap.GetTile(cellPos) == null;
                if (!walkable) unwalkableCount++;
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
        Debug.Log("Unwalkable nodes: " + unwalkableCount);
    }
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01(
            (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentY = Mathf.Clamp01(
            (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX && 
                    checkY >= 0 && checkY < gridSizeY)
                    neighbours.Add(grid[checkX, checkY]);
            }
        }
        return neighbours;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, 
            new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = n.walkable ? Color.white : Color.red;
                Vector3 pos = new Vector3(n.worldPosition.x, n.worldPosition.y, -1);
                Gizmos.DrawCube(pos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX, gridY;
    public int gCost, hCost;
    public Node parent;

    public int fCost { get { return gCost + hCost; } }

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}