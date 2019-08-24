using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;

		//Debug.Log("x: " + transform.position.x + ", z: " + transform.position.y);
		// Drop on grid based on (x,y) pos
        // Have to translate (x,y) pos to (x,z) pos on board
	}
}
