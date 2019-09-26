using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.UI;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    private readonly string CLASS_NAME = "CreateRoom";

    [SerializeField]
    private Text _roomName;

    [SerializeField]
    private byte _maxPlayers;
    public int MaxPlayers => _maxPlayers;

    private CanvasesManager _canvasesManager;

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        RoomOptions options = new RoomOptions { MaxPlayers = _maxPlayers };
        PhotonNetwork.CreateRoom(_roomName.text, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.LogFormat("{0}: Room '{1}' created successfully.", 
            CLASS_NAME, _roomName.text);
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
