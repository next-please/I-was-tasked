using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;

public class CameraController : MonoBehaviour
{
    public Transform[] CameraTransforms;
    private Transform playerTransform;

    float speed = 1f;
    int playerPosition = 0;

    void Awake()
    {
        playerTransform = CameraTransforms[(int)RoomManager.GetLocalPlayer()];
        playerPosition = -1; // -1 is the market
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && playerPosition + 1 < 3 && playerPosition >= 0)
        {
            StopAllCoroutines();
            StartCoroutine(LerpToTransform(CameraTransforms[playerPosition + 1]));
            playerPosition++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && playerPosition - 1 >= 0)
        {
            StopAllCoroutines();
            StartCoroutine(LerpToTransform(CameraTransforms[playerPosition - 1]));
            playerPosition--;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && playerPosition != -1)
        {
            StopAllCoroutines();
            StartCoroutine(LerpToTransform(CameraTransforms[3]));
            playerPosition = -1;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && playerPosition == -1)
        {
            StopAllCoroutines();
            StartCoroutine(LerpToTransform(playerTransform));
            playerPosition = (int)RoomManager.GetLocalPlayer();
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
