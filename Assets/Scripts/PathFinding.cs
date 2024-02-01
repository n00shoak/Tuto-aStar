using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager pathRequestManager;
    public Grid grid;

    void Awake()
    {
        pathRequestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>(); // get grid component in the same game object
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }


    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathFound = false;

        //convert position in world of target to position in grid
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode); // add the starting point to the list of node to search in

            while (openSet.Count > 0)
            {
                Node node = openSet.RemoveFirst(); ;

                closedSet.Add(node);

                if (node == targetNode) // path to target is found
                {
                    pathFound = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(node)) // for each valid neighbouring node around the target node 
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) // if neigbour is already searched or not walkable then you dont search it
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.gCost + GetDistance(node, neighbour); // calculate the cost of neighbour
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour; // set g cost (cost by distance between neighbourin node)
                        neighbour.hCost = GetDistance(neighbour, targetNode); // set cost by the distance between target and this node
                        neighbour.parent = node;   // the best way to go to the neighbour is from node

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        else { Debug.LogWarning("start position or destination is unwalkable");}

       
        yield return null;
        if(pathFound)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        pathRequestManager.FinishedProcessingPath(waypoints, pathFound);


    }

    //once the search arrived to the target ; create a list of the best node used for the path
    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        // only place waypoint when path change
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
        
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();

        for(int i = 1; i< path.Count; i++)
        {
            waypoints.Add(path[i].worldPosition - transform.position);
        }
        return waypoints.ToArray();
    }

    //get distance between two given node
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}