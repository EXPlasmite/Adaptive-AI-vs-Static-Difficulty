using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

// Generates and stores the grid of nodes used by the A* pathfinding system.
// Nodes are marked walkable or unwalkable based on the wall tilemap.
// Built once at scene load and remains static throughout the session.
public class PathfindingGrid : MonoBehaviour
{
    public static PathfindingGrid Instance;

    public Tilemap wallTilemap;   // Reference to the wall tilemap in the scene
    public Vector2 gridWorldSize; // Total world size the grid should cover
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
    }

    // Creates a 2D array of nodes covering the grid world size.
    // Each node is marked walkable if no wall tile occupies that position.
    void CreateGrid()
    {
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

                // Node is walkable if no wall tile exists at this position
                Vector3Int cellPos = wallTilemap.WorldToCell(worldPoint);
                bool walkable = wallTilemap.GetTile(cellPos) == null;
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // Converts a world position to the nearest grid node
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

    // Returns all 8 surrounding neighbours of a node (including diagonals)
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // Skip the node itself
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX &&
                    checkY >= 0 && checkY < gridSizeY)
                    neighbours.Add(grid[checkX, checkY]);
            }
        }
        return neighbours;
    }

    // Returns all neighbours within a given radius - used by Pathfinding
    // to find the nearest walkable node when start or target is unwalkable
    public List<Node> GetNeighboursInRadius(Node node, int radius)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
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
}

// Represents a single node in the pathfinding grid.
// fCost = gCost (distance from start) + hCost (estimated distance to target)
public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX, gridY;
    public int gCost, hCost;
    public Node parent; // Used to retrace the path once target is reached

    public int fCost { get { return gCost + hCost; } }

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}