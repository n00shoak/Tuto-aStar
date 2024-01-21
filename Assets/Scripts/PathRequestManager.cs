using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;
    Pathfinding pathfinding;

    bool isProcessingPath;

    static PathRequestManager instance;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 pathStart,Vector3 pathEnd , Action<Vector3[] , bool> callBack)
    {
        PathRequest newRequest = new PathRequest(pathStart,pathEnd,callBack);
        instance.pathRequestQueue.Enqueue(newRequest);   // add stuf to the queue
        instance.TryProcessNext();
    }

    void TryProcessNext() // if not currently processing a path >> process next one in queue
    {
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue(); // get and take out the first item in the queue
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart,currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path,bool success)
    {
        currentPathRequest.callBack(path,success);
        isProcessingPath =false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callBack;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callBack)
        {
            pathStart = _start;
            pathEnd = _end;

            callBack = _callBack;
        }
    }
}
