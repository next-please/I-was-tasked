﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasesManager : MonoBehaviour
{
    [SerializeField]
    private CreateOrJoinRoomCanvas _createRoomCanvas; 
    public CreateOrJoinRoomCanvas CreateRoomCanvas { get { return _createRoomCanvas; } }

    [SerializeField]
    private CurrentRoomCanvas _currentRoomCanvas;
    public CurrentRoomCanvas CurrentRoomCanvas { get { return _currentRoomCanvas;  } }

    private void Awake()
    {
        FirstInitialize();
    }

    private void FirstInitialize()
    {
        CreateRoomCanvas.FirstInitialize(this);
        CurrentRoomCanvas.FirstInitialize(this);
    }


}
