using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDragHandler : MonoBehaviour
{
    private float zPos;

    void OnMouseDown()
    {
        zPos = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    void OnMouseUp()
    {
        Debug.Log("x: " + transform.position.x + ", z: " + transform.position.z);
        // Drop on grid based on (x,z) pos
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = zPos;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
