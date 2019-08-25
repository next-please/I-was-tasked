using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
	public PieceDetector pieceDetector;


	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		transform.localPosition = Vector3.zero;
		pieceDetector.addPiece(tag);
	}
}
