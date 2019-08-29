using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectDragHandler : MonoBehaviour
{
    public UnityEvent onDrop;

    private float zPos;
    private Vector3 originalPos;

    void OnMouseDown()
    {
        originalPos = transform.position;
        zPos = Camera.main.WorldToScreenPoint(transform.position).z;

        EventManager.Instance.draggedPiece = gameObject.GetComponent<PieceView>().piece;

        // switch to dragging view
        gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    // TODO: notified by dropped event instead of using mouse up
    void OnMouseUp()
    {
        Debug.Log("drag handler: " + EventManager.Instance.isPieceDropped);
        if (EventManager.Instance.isPieceDropped)
        {
            Debug.Log("destroy");
            Destroy(gameObject);
            EventManager.Instance.isPieceDropped = false;
        }
        else
        {
            Debug.Log("return to pos");
            gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            gameObject.GetComponent<Collider>().enabled = true;
            transform.position = originalPos;
        }
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
