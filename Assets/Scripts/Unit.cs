using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public Transform shipPosition;
    public float speed = 5;
    Vector3[] path;
    int targetIndex;

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position - shipPosition.position, target.position - shipPosition.position,OnPathFound);
    }

    public void ChangePath()
    {
        PathRequestManager.RequestPath(transform.position - shipPosition.position, target.position - shipPosition.position, OnPathFound);

    }

    public void OnPathFound(Vector3[] newPath,bool pathSuccessful)
    {
        
        if(pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine(followPath());
            StartCoroutine(followPath());
        }
    }

    IEnumerator followPath()
    {
        while(true)
        {
            if (targetIndex >= path.Length)
            {
                yield break;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, path[targetIndex] + shipPosition.position, speed);

                if (transform.position == path[targetIndex] + shipPosition.position)
                {
                    targetIndex++; // go to nex waypoint

                }
                yield return null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i] + shipPosition.position, Vector3.one);
                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i] + shipPosition.position);
                }
                else
                {
                    Gizmos.DrawLine(path[i-1] + shipPosition.position, path[i] + shipPosition.position);
                }
            }
        }
    }
}
