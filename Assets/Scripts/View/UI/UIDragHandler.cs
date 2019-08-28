using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIDragHandler : MonoBehaviour
{
    public UnityEvent onDrag;
    public UnityEvent onDrop;

    // public void OnDrag(PointerEventData eventData)
    // {
    //     transform.position = Input.mousePosition;
    //     onDrag.Invoke();
    // }

    // public void OnEndDrag(PointerEventData eventData)
    // {
    //     transform.localPosition = Vector3.zero;
    //     onDrop.Invoke();
    // }
}
