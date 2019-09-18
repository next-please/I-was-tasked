using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    int count = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && count < 2)
        {
            transform.position += Vector3.right * 20;
            count++;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && count > 0)
        {
            transform.position += Vector3.left * 20;
            count--;
        }
    }
}
