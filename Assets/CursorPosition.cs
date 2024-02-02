using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPosition : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask tileMapMask;

    private void LateUpdate()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, cam.transform.position.y + 5, tileMapMask))
        {
            transform.position = new Vector3(Mathf.RoundToInt(hit.point.x), 0, Mathf.RoundToInt(hit.point.z));
        }
    }

}
