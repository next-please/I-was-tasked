using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDetector : MonoBehaviour
{
    private RaycastHit hit;
    private Ray ray;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addPiece(string name)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {

            Debug.Log("hit point: " + hit.point);

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = hit.point;

            // TODO: Snap to grid

            Debug.Log("piece added: " + name);
        }
    }
}
