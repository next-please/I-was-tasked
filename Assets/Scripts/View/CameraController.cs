using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;

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

public class CameraViewOwnBoardEvent : GameEvent
{
    public CameraView targetView;
}

public class CameraController : MonoBehaviour
{
    public Transform[] CameraTransforms;
    public LayerMask layerMask;

    private Transform playerTransform;
    private float speed = 1f;

    public Camera MainCamera;
    public Camera FreeRoamCamera;
    public GameObject UI;
    public Volume volume;

    static public LayerMask _layerMask;

    static int playerPosition = 0;

    void Awake()
    {
        playerTransform = CameraTransforms[(int)RoomManager.GetLocalPlayer()];
        playerPosition = -1; // -1 is the market
        _layerMask = layerMask;
        FreeRoamCamera.enabled = false;
        UI.SetActive(true);
    }

    void ToggleCameras()
    {
        FreeRoamCamera.enabled = !FreeRoamCamera.enabled;
        UI.SetActive(!UI.activeSelf);

        DepthOfField dof = null;
        if (volume.profile.TryGet<DepthOfField>(out dof))
        {
            dof.active = !FreeRoamCamera.enabled;
        }

        GameObject[] allObjects = (GameObject[]) FindObjectsOfType(typeof(GameObject));
        foreach (GameObject gameObject in allObjects)
        {
            if (gameObject.GetComponent<PieceView>() != null)
            {
                PieceView pv = gameObject.GetComponent<PieceView>();
                float toggleX = (FreeRoamCamera.enabled) ? 0 : 0.07f;
                pv.statusBars.transform.localScale = new Vector3(toggleX, pv.statusBars.transform.localScale.y, pv.statusBars.transform.localScale.z);
                GameObject[] rarities = gameObject.GetComponent<PieceView>().rarities;
                if (rarities != null && rarities[pv.piece.GetRarity() - 1] != null)
                {
                    rarities[pv.piece.GetRarity() - 1].SetActive(!FreeRoamCamera.enabled);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Hello");
            ToggleCameras();
            return;
        }

        if (MainCamera.enabled)
        {
            if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && playerPosition + 1 < 3 && playerPosition >= 0)
            {
                StopAllCoroutines();
                StartCoroutine(LerpToTransform(CameraTransforms[playerPosition + 1]));
                playerPosition++;
                EventManager.Instance.Raise(new CameraPanEvent { targetView = (CameraView)playerPosition });
            }
            else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && playerPosition - 1 >= 0)
            {
                StopAllCoroutines();
                StartCoroutine(LerpToTransform(CameraTransforms[playerPosition - 1]));
                playerPosition--;
                EventManager.Instance.Raise(new CameraPanEvent { targetView = (CameraView)playerPosition });
            }

            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && playerPosition != -1)
            {
                StopAllCoroutines();
                StartCoroutine(LerpToTransform(CameraTransforms[3]));
                playerPosition = -1;
                EventManager.Instance.Raise(new CameraPanEvent { targetView = (CameraView)playerPosition });
            }

            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && playerPosition == -1)
            {
                StopAllCoroutines();
                StartCoroutine(LerpToTransform(playerTransform));
                playerPosition = (int)RoomManager.GetLocalPlayer();
                EventManager.Instance.Raise(new CameraPanEvent { targetView = (CameraView)playerPosition });
            }
        }
    }

    public static bool IsViewingOwnBoard()
    {
        return playerPosition == (int)RoomManager.GetLocalPlayer();
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

    public static Vector3 GetMousePositionOnBoard(ref bool wasHit)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, _layerMask))
        {
            wasHit = true;
            return hit.point;
        }
        wasHit = false;
        return Vector3.zero;
    }
}
