using UnityEngine;
using System.Collections;

public class Node
{

    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;  // position X on grid
    public int gridY;  // position Y on grid

    public int gCost;  // cost to go to neighbour
    public int hCost;  // cost = distance to target
    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost; // total cost to move
        }
    }
}