using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    AStarAlgorithm aStarAlgorithm;

    bool isProcessingPath;

    void Awake() 
    {
        instance = this;
        aStarAlgorithm = GetComponent<AStarAlgorithm>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, GameObject requester, Action<Vector3[], bool> callback)
    {
		PathRequest newRequest = new PathRequest(pathStart, pathEnd, requester, callback);
		instance.pathRequestQueue.Enqueue(newRequest);
        
        PathRequest currentRequest = instance.pathRequestQueue.Peek();
        Debug.Log("Requester: " + requester.name + ", Start: " + currentRequest.pathStart + ", End: " + currentRequest.pathEnd);

		instance.TryProcessNext();
	}

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            aStarAlgorithm.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
    
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public GameObject requester;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, GameObject _requester, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            requester = _requester;
            callback = _callback;
        }
    }
}