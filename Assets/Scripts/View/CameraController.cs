using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;

public enum CameraView
{
    PlayerOne = 0,
    PlayerTwo = 1,
    PlayerThree = 2,
    Market = -1
}

public class CameraPanEvent : GameEvent
{
    public CameraView targetView;
}

public class CameraController : MonoBehaviour
{
    float speed = 1f;
    public Transform[] CameraTransforms;

    private Transform playerTransform;
    private int playerPosition = 0;

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
            EventManager.Instance.Raise(new CameraPanEvent { targetView = (CameraView)playerPosition });
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && playerPosition - 1 >= 0)
        {
            StopAllCoroutines();
            StartCoroutine(LerpToTransform(CameraTransforms[playerPosition - 1]));
            playerPosition--;
            EventManager.Instance.Raise(new CameraPanEvent { targetView = (CameraView)playerPosition });
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && playerPosition != -1)
        {
            StopAllCoroutines();
            StartCoroutine(LerpToTransform(CameraTransforms[3]));
            playerPosition = -1;
            EventManager.Instance.Raise(new CameraPanEvent { targetView = (CameraView)playerPosition });
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && playerPosition == -1)
        {
            StopAllCoroutines();
            StartCoroutine(LerpToTransform(playerTransform));
            playerPosition = (int)RoomManager.GetLocalPlayer();
            EventManager.Instance.Raise(new CameraPanEvent { targetView = (CameraView)playerPosition });
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
