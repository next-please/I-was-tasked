using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    private readonly string CLASS_NAME = "CreateRoom";

    [SerializeField]
    private TextMeshProUGUI _roomName;

    [SerializeField]
    private TextMeshProUGUI _numPlayersInput;

    private CanvasesManager _canvasesManager;

    private bool _isTutorial = true;

    private readonly int defaultNumOfPlayers = 3;
    private readonly int maxNumOfPlayers = 3;

    private int attempt = 0;
    private RoomOptions options;
    private string roomName;

    private void Awake()
    {
    }

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        byte maxPlayers = 3;
        Debug.Log(_numPlayersInput.text.Length);
        if (_numPlayersInput.text.Length > 1)
        {
            try
            {
                int numOfPlayers = Convert.ToInt32(_numPlayersInput.text.Substring(0, _numPlayersInput.text.Length - 1));
                if ( numOfPlayers < 1 || numOfPlayers > 3)
                {
                    numOfPlayers = defaultNumOfPlayers;
                }
                maxPlayers = Convert.ToByte(numOfPlayers);
            } catch (Exception e)
            {
                Debug.LogFormat("{0}: Failed to create room - number of players not an integer", CLASS_NAME);
                return;
            }
        }
        else
        {
            maxPlayers = Convert.ToByte(defaultNumOfPlayers);
        }

        // Empty Room Name
        options = new RoomOptions { MaxPlayers = maxPlayers, PublishUserId = true };
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {{ "isTutorial", _isTutorial }};
        roomName = (_roomName.text.Length <= 1) ? PhotonNetwork.LocalPlayer.NickName + "'s Room" : _roomName.text;
        
        PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
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
        attempt++;
        PhotonNetwork.CreateRoom(roomName + " (" + attempt + ")", options, TypedLobby.Default);
    }

    public void FirstInitialize(CanvasesManager mg)
    {
        _canvasesManager = mg;
    }
}
