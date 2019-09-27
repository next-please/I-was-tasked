using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks
{
    private string CLASS_NAME = "TestConnect";
    void Start()
    {
        Debug.LogFormat("{0}: Connecting to Server.", CLASS_NAME);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.NickName = "User#" + Random.Range(0, 9999);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogFormat("{0}: {1} is connected to Server.", 
            CLASS_NAME, PhotonNetwork.NickName);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogFormat("{0}: Disconnected from Server - {1}", CLASS_NAME, cause);
    }

    public override void OnJoinedLobby()
    {
        Debug.LogFormat("{0}: Sucessfully joined lobby.", CLASS_NAME);
        Debug.LogFormat("{0}: Lobby name - {1}",
            CLASS_NAME, PhotonNetwork.CurrentLobby.Name);
    }
}
