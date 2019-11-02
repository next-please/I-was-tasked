using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    private readonly string CLASS_NAME = "CreateRoom";

    [SerializeField]
    private TextMeshProUGUI _roomName;

    [SerializeField]
    private TextMeshProUGUI _numPlayersInput;

    private CanvasesManager _canvasesManager;

    private void Awake()
    {
        _numPlayersInput.text = "3 ";
    }

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        byte maxPlayers = 3;
        try
        {
            maxPlayers = Convert.ToByte(_numPlayersInput.text.Substring(0, _numPlayersInput.text.Length - 1));
        } catch (Exception e)
        {
            Debug.LogFormat("{0}: Failed to create room - number of players not an integer", CLASS_NAME);
            return;
        }

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers };
        PhotonNetwork.CreateRoom(_roomName.text, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.LogFormat("{0}: Room '{1}' created successfully with maximum of {2} players",
            CLASS_NAME, _roomName.text, PhotonNetwork.CurrentRoom.MaxPlayers);
        _canvasesManager.CurrentRoomCanvas.Show();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogFormat("{0}: Room '{1}' creation was unsuccessful - {2}",
            CLASS_NAME, _roomName.text, message);
    }

    public void FirstInitialize(CanvasesManager mg)
    {
        _canvasesManager = mg;
    }

}
