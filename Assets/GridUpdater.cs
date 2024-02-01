using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridUpdater : MonoBehaviour
{
    [SerializeField] Pathfinding getGrid;
    [SerializeField] Grid grid;
    [SerializeField] Unit[] AllConcernedUnit;

    public bool Activate;

    private void Start()
    {
        grid = getGrid.grid;
    }

    private void Update()
    {
        if(Activate)
        {
            grid.CreateGrid();
            for (int i = 0; i < AllConcernedUnit.Length; i++)
            {
                AllConcernedUnit[i].ChangePath();
            }
            Activate = false;
        }
    }
}
