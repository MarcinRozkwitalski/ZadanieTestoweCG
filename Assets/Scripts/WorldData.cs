using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    public LayerMask obstacleMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    
    float _nodeDiameter;
    int _gridSizeX, _gridSizeY;

    Node[,] mapGrid;

    bool _showGizmos = true;

    void Start()
    {
        _nodeDiameter = nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);

        InitialCheckForObstacles();
    }

    public void InitialCheckForObstacles()
    {
        mapGrid = new Node[_gridSizeX, _gridSizeY];
        
        Vector3 xAxisOffset = Vector3.right * gridWorldSize.x / 2;
        Vector3 zAxisOffset = Vector3.forward * gridWorldSize.y / 2;
        Vector3 worldBottomLeft = transform.position - xAxisOffset - zAxisOffset;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft +  
                                        Vector3.right * (x * _nodeDiameter + nodeRadius) + 
                                        Vector3.forward * (y * _nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckBox(worldPoint, Vector3.one * nodeRadius, Quaternion.identity, obstacleMask);
                
                mapGrid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }

    private void OnDrawGizmos() 
    {
        if (!_showGizmos)
            return;

        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, .2f, gridWorldSize.y));

        if (mapGrid != null)
        {
            foreach (Node n in mapGrid)
            {
                if (n.walkable) Gizmos.color = Color.green;
                else Gizmos.color = Color.red;
                Gizmos.DrawCube(n.worldPosition, new Vector3(1, .2f, 1) * (_nodeDiameter));
            }
        }
    }

    public void ToggleGizmosVisibility()
    {
        _showGizmos = !_showGizmos;
    }
}