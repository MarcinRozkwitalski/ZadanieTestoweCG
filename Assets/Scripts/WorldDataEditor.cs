using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(WorldData))]
public class WorldDataEditor : Editor
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();

        WorldData worldData = (WorldData)target;

        if (GUILayout.Button("Regenerate Grid")) worldData.InitialCheckForObstacles();

        if (GUILayout.Button("Toggle Gizmos Visibility")) worldData.ToggleGizmosVisibility();
    }
}