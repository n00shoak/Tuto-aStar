using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildScript : MonoBehaviour
{
    public GameObject SelectedBlock;
    public List<GameObject> savedObjects;
    Vector3 oldPosition;

    private void Update()
    {
        GameObject NewTile;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            NewTile = Instantiate(SelectedBlock, transform);
            NewTile.transform.parent = null;
            savedObjects.Add(NewTile);
        }
        if (Input.GetKey(KeyCode.Mouse0) && transform.position != oldPosition)
        {
            NewTile = Instantiate(SelectedBlock, transform);
            NewTile.transform.parent = null;
            savedObjects.Add(NewTile);
        }

        oldPosition = transform.position;
    }
}
