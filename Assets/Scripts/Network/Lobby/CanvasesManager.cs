using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasesManager : MonoBehaviour
{
    [SerializeField]
    private ConnectCanvas _connectCanvas;
    public ConnectCanvas ConnectCanvas { get { return _connectCanvas; } }

    [SerializeField]
    private CreateOrJoinRoomCanvas _createOrJoinRoomCanvas; 
    public CreateOrJoinRoomCanvas CreateOrJoinRoomCanvas { get { return _createOrJoinRoomCanvas; } }

    [SerializeField]
    private CurrentRoomCanvas _currentRoomCanvas;
    public CurrentRoomCanvas CurrentRoomCanvas { get { return _currentRoomCanvas;  } }

    private void Awake()
    {
        FirstInitialize();
    }

    private void FirstInitialize()
    {
        ConnectCanvas.FirstInitialize(this);
        CreateOrJoinRoomCanvas.FirstInitialize(this);
        CurrentRoomCanvas.FirstInitialize(this);
    }


}
