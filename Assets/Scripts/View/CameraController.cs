using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] CameraTransforms;
    private Transform playerTransform;
    private Transform currTransform;

    float speed = 1f;
    int count = 0;

    void Awake()
    {
        playerTransform = CameraTransforms[0];
        currTransform = transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && count + 1 < 3)
        {
            StopAllCoroutines();
            StartCoroutine(LerpToTransform(CameraTransforms[count + 1]));
            count++;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && count - 1 >= 0)
        {
            StopAllCoroutines();
            StartCoroutine(LerpToTransform(CameraTransforms[count - 1]));
            count--;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && currTransform != CameraTransforms[3])
        {
            StopAllCoroutines();
            StartCoroutine(LerpToTransform(CameraTransforms[3]));
            currTransform = CameraTransforms[3];
            count = -1;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && currTransform != playerTransform)
        {
            StopAllCoroutines();
            StartCoroutine(LerpToTransform(playerTransform));
            currTransform = playerTransform;
            count = 0;
        }
    }

    IEnumerator LerpToTransform(Transform newTransform)
    {
        float duration = 1 / speed;
        Transform currentTransform = transform;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            transform.position = Vector3.Slerp(currentTransform.position, newTransform.position, t / duration);
            transform.rotation = Quaternion.Slerp(currentTransform.rotation, newTransform.rotation, t / duration);
            yield return 0;
        }
    }
}
