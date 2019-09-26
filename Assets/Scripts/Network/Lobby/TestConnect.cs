using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks
{
    private string CLASS_NAME = "TestConnect";
    void Start()
    {
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.NickName = "User#" + Random.Range(0, 9999);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogFormat("{0}: {1} is connected to Server.", 
            CLASS_NAME, PhotonNetwork.NickName);
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
