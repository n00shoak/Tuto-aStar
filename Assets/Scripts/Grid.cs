using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public bool drawGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        // set grid size
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));  // if there is an object on the grid , node overlapping the object are unwalkable
                grid[x, y] = new Node(walkable, worldPoint, x , y);
            }
        }

        Debug.Log(grid[0,0].worldPosition);
    }

    // check neighbour cost if there is neighbour
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) // do not count self
                    continue;

                // node position on grid
                int checkX = node.gridX + x; 
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) // check if not out of the border of the grid
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }


           
        // !!! not from the tutorial !!!     (its entended to be clear rather than optimized)
        //once we checked wich node is available for mevement , we retrieve the corner to move make the search move around walls
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) // do not count self
                    continue;

                // node position on grid
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 1 && checkX < gridSizeX-1 && checkY >= 1 && checkY < gridSizeY-1) // don't check out of borders
                {
                    if (y == 0 && x != y) // tile to the left & right
                    {
                        if (!grid[checkX, checkY].walkable) // if the tile is not walkable , we disable the adjacent corner
                        {
                            neighbours.Remove(grid[checkX, checkY + 1]);
                            neighbours.Remove(grid[checkX, checkY + -1]);
                        }
                    }

                    if (x == 0 && y != x) // tile to the left & right
                    {
                        if (!grid[checkX, checkY].walkable) // if the tile is not walkable , we disable the adjacent corner
                        {
                            neighbours.Remove(grid[checkX + 1, checkY]);
                            neighbours.Remove(grid[checkX - 1, checkY]);
                        }
                    }
                }
            }
        }
        return neighbours;
    }

    //convert position in the world to node position
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null && drawGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red; // if node is wwalkable : color = white else color = red
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}