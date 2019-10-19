using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ConnectCanvas : MonoBehaviourPunCallbacks
{
    private string CLASS_NAME = "ConnectCanvas";

    [SerializeField]
    private Text _nickname;

    [SerializeField]
    private GameObject _connectMenu;

    [SerializeField]
    private GameObject _connectingText;

    [SerializeField]
    private CanvasesManager _canvasesManager;

    public void FirstInitialize(CanvasesManager mg)
    {
        _canvasesManager = mg; 
    }

    public void OnClick_SinglePlayer()
    {
        SceneManager.LoadScene("Main Scene MP");
    }

    public void OnClick_Connect()
    {
        Debug.LogFormat("{0}: Connecting to Server.", CLASS_NAME);
        if (String.IsNullOrEmpty(_nickname.text))
        {
            Debug.LogFormat("{0}: Connection to Server Failed - Nickname provided is empty.", CLASS_NAME);
            // TODO: show error text on UI
            return;
        }
        _connectMenu.gameObject.SetActive(false);
        _connectingText.SetActive(true);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.NickName = _nickname.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
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
        _connectingText.SetActive(false);
        _canvasesManager.CreateOrJoinRoomCanvas.Show();
    }
}
