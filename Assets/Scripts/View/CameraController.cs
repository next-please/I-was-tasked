using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private readonly Vector3 marketPosition = new Vector3(23.4f, 10.7f, -28f);

    private bool isMoving = false;
    private Vector3 playerPosition;
    private Vector3 currPosition;

    float speed = 1f;
    int count = 0;

    void Awake()
    {
        playerPosition = transform.position;
        currPosition = playerPosition;
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && count < 2)
            {
                StartCoroutine(LerpToPosition(transform.position + Vector3.right * 20));
                count++;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && count > 0)
            {
                StartCoroutine(LerpToPosition(transform.position + Vector3.left * 20));
                count--;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && currPosition != marketPosition)
            {
                StartCoroutine(LerpToPosition(marketPosition));
                currPosition = marketPosition;
            }

            if (Input.GetKeyUp(KeyCode.UpArrow) && currPosition != playerPosition)
            {
                StartCoroutine(LerpToPosition(playerPosition));
                currPosition = playerPosition;
            }
        }
    }

    IEnumerator LerpToPosition(Vector3 newPosition)
    {
        isMoving = true;
        float duration = 1 / speed;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, t / duration);
            yield return 0;
        }
        isMoving = false;
    }
}
