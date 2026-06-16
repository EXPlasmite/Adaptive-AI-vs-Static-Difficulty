using UnityEngine;
using System.Collections.Generic;

// A* pathfinding implementation used by all enemies to navigate
// towards the player while avoiding unwalkable wall tiles.
// Identical in both static and adaptive conditions.
public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance;

    void Awake()
    {
        Instance = this;
    }

    // Finds the shortest walkable path from startPos to targetPos.
    // Returns null if no path exists.
    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = PathfindingGrid.Instance.NodeFromWorldPoint(startPos);
        Node targetNode = PathfindingGrid.Instance.NodeFromWorldPoint(targetPos);

        // If start or target falls on an unwalkable node (e.g. a wall tile),
        // find the nearest walkable node within a search radius of up to 4
        if (!targetNode.walkable)
            targetNode = GetNearestWalkableNode(targetNode);

        if (!startNode.walkable)
            startNode = GetNearestWalkableNode(startNode);

        if (targetNode == null || startNode == null)
            return null;

        // A* open and closed sets
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // Find lowest fCost node in open set (tiebreak on hCost)
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    openSet[i].fCost == currentNode.fCost &&
                    openSet[i].hCost < currentNode.hCost)
                    currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // Path found - retrace and return
            if (currentNode == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbour in
                PathfindingGrid.Instance.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost +
                    GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost ||
                    !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null; // No path found
    }

    // Searches outward in increasing radii to find the nearest walkable node
    Node GetNearestWalkableNode(Node node)
    {
        for (int radius = 1; radius < 5; radius++)
        {
            List<Node> neighbours = PathfindingGrid.Instance.GetNeighboursInRadius(node, radius);
            foreach (Node neighbour in neighbours)
            {
                if (neighbour.walkable)
                    return neighbour;
            }
        }
        return null;
    }

    // Traces back from end node to start node via parent references
    // and reverses to produce a start-to-end path
    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    // Calculates movement cost between two nodes.
    // Diagonal moves cost 14, straight moves cost 10
    // (approximating sqrt(2) * 10 for diagonal distance)
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return dstX > dstY ? 14 * dstY + 10 * (dstX - dstY) :
                             14 * dstX + 10 * (dstY - dstX);
    }
}