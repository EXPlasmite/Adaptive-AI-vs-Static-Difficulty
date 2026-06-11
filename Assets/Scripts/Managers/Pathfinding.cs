using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance;

    void Awake()
    {
        Instance = this;
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = PathfindingGrid.Instance.NodeFromWorldPoint(startPos);
        Node targetNode = PathfindingGrid.Instance.NodeFromWorldPoint(targetPos);

        if (!targetNode.walkable)
            targetNode = GetNearestWalkableNode(targetNode);

        if (!startNode.walkable)
            startNode = GetNearestWalkableNode(startNode);

        if (targetNode == null || startNode == null)
            return null;

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
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
        return null;
    }

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

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return dstX > dstY ? 14 * dstY + 10 * (dstX - dstY) : 
                             14 * dstX + 10 * (dstY - dstX);
    }
}