using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{

    public Transform seeker, target;
    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>(); // get grid component in the same game object
    }

    void Update()
    {
        FindPath(seeker.position, target.position);  // get starting point and destination point
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //convert position in world of target to position in grid
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode); // add the starting point to the list of node to search in

        while (openSet.Count > 0)
        {
            Node node = openSet.RemoveFirst(); ;
           
            closedSet.Add(node);

            if (node == targetNode) // if node to search in is the destination , then you got to show the path
            {
                RetracePath(startNode, targetNode);
                return;
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
                }
            }
        }
    }

    //once the search arrived to the target ; create a list of the best node used for the path
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;

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