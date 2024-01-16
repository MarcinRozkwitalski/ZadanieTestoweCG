using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    public Transform characterOne, characterTwo, characterThree, characterFour;
    public LayerMask obstacleMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    
    float _nodeDiameter;
    int _gridSizeX, _gridSizeY;

    Node[,] mapGrid;

    bool _showGizmos = true;

    void Awake()
    {
        _nodeDiameter = nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);

        InitialCheckForObstacles();
    }

    public int MaxSize
    {
        get
        {
            return _gridSizeX * _gridSizeY;
        }
    }

    public void InitialCheckForObstacles()
    {
        mapGrid = new Node[_gridSizeX, _gridSizeY];
        
        Vector3 xAxisOffset = Vector3.right * gridWorldSize.x / 2,
                zAxisOffset = Vector3.forward * gridWorldSize.y / 2,
                worldBottomLeft = transform.position - xAxisOffset - zAxisOffset;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft +  
                                     Vector3.right * (x * _nodeDiameter + nodeRadius) + 
                                     Vector3.forward * (y * _nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckBox(worldPoint, Vector3.one * nodeRadius, Quaternion.identity, obstacleMask);
                
                mapGrid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x, 
                    checkY = node.gridY + y;

                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                {
                    neighbours.Add(mapGrid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node GetNodePosition(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x,
              percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        int x = Mathf.FloorToInt(Mathf.Clamp(_gridSizeX * percentX, 0, _gridSizeX - 1)),
            y = Mathf.FloorToInt(Mathf.Clamp(_gridSizeY * percentY, 0, _gridSizeY - 1));

        return mapGrid[x, y];
    }

    private void OnDrawGizmos() 
    {
        if (!_showGizmos)
            return;

        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, .2f, gridWorldSize.y));

        if (mapGrid != null)
        {
            Node characterOneNode = GetNodePosition(characterOne.position), 
                 characterTwoNode = GetNodePosition(characterTwo.position),
                 characterThreeNode = GetNodePosition(characterThree.position),
                 characterFourNode = GetNodePosition(characterFour.position);

            foreach (Node n in mapGrid)
            {
                if (n.walkable) Gizmos.color = Color.green;
                else Gizmos.color = Color.red;

                if (characterOneNode == n || 
                    characterTwoNode == n || 
                    characterThreeNode == n || 
                    characterFourNode == n) 
                    Gizmos.color = Color.white;

                Gizmos.DrawCube(n.worldPosition, new Vector3(1, .2f, 1) * _nodeDiameter);
            }
        }
    }

    public void ToggleGizmosVisibility()
    {
        _showGizmos = !_showGizmos;
    }
}