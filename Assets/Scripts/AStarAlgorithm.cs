using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Unity.VisualScripting;
using System.Linq;

public class AStarAlgorithm : MonoBehaviour
{
    PathRequestManager pathRequestManager;
    WorldData worldData;

    public List<Coroutine> activeCoroutines = new List<Coroutine>();
    
    void Awake()
    {
        pathRequestManager = GetComponent<PathRequestManager>();
        worldData = GetComponent<WorldData>();
    }

    public void StartFindPath(Vector3 startPosition, Vector3 endPosition)
    {
        StartCoroutine(CalculatePath(startPosition, endPosition));
    }

    IEnumerator CalculatePath(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = worldData.GetNodePosition(startPosition);
        Node endNode = worldData.GetNodePosition(endPosition);

        if (endNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(worldData.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                
                closedSet.Add(currentNode);

                if (currentNode == endNode) 
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in worldData.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || 
                        closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                    if (newMovementCostToNeighbour < neighbour.gCost || 
                        !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, endNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, endNode);
        }
        pathRequestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path, startNode, endNode);
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path, Node startNode, Node endNode)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i-1].worldPosition);
            }
            directionOld = directionNew;
            if (i == path.Count - 1 && directionOld != new Vector2(path[i].gridX, path[i].gridY) - new Vector2(startNode.gridX, startNode.gridY))
                waypoints.Add(path[i-1].worldPosition);
        }

        if (waypoints.Count < 1)
        {
            waypoints.Add(endNode.worldPosition);
        }

        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX),
            distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        
        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);

        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}