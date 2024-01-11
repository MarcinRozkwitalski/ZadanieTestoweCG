using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    public int[,] MapGrid = new int[100, 100];

    void Start()
    {
        InitialCheckForObstacles();
    }

    void Update()
    {
        
    }

    public void InitialCheckForObstacles()
    {
        
    }
}