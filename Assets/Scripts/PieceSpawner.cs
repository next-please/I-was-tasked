using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public float gridSize = 1f;
    public float y = 1f;

    private RaycastHit hit;
    private Ray ray;

    // TODO: use piece class instead of string?
    public void SpawnPiece(string name)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("hit point: " + hit.point);

            // TODO: check if tile is occupied before spawning

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = GetSnapPoint(hit.point);

            Debug.Log("piece added: " + name);
        }
    }

    private Vector3 GetSnapPoint(Vector3 point)
    {
        int x = Mathf.RoundToInt(hit.point.x / gridSize);
        int z = Mathf.RoundToInt(hit.point.z / gridSize);

        return new Vector3(x * gridSize, y, z * gridSize);
    }
}
