using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectDragHandler : MonoBehaviour
{
    public UnityEvent onDrop;

    private float zPos;

    void OnMouseDown()
    {
        zPos = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    void OnMouseUp()
    {
        //onDrop.Invoke();
        //Destroy(gameObject);
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
