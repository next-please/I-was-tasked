using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    public Button tutorialButton;
    public TextMeshProUGUI tutorialTextNormal;
    public TextMeshProUGUI tutorialTextPressed;

    private readonly string CLASS_NAME = "CreateRoom";

    [SerializeField]
    private TextMeshProUGUI _roomName;

    [SerializeField]
    private TextMeshProUGUI _numPlayersInput;

    private CanvasesManager _canvasesManager;

    private bool _isTutorial = true;

    private void Awake()
    {
        _numPlayersInput.text = "3 ";
        _roomName.text = "";
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

        // Empty Room Name
        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers };
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {{ "isTutorial", _isTutorial }};
        string roomName = (_roomName.text.Length <= 1) ? PhotonNetwork.LocalPlayer.NickName + "'s Room" : _roomName.text;
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
    }

    public void FirstInitialize(CanvasesManager mg)
    {
        _canvasesManager = mg;
    }

    public void OnClick_Tutorial()
    {
        if (_isTutorial)
        {
            DisableTutorial();
        }
        else
        {
            EnableTutorial();
        }
    }

    void EnableTutorial()
    {
        ColorBlock cb = tutorialButton.colors;
        cb.normalColor = new Color(0.0f, 0.7f, 0.0f);
        cb.selectedColor = cb.normalColor;
        cb.highlightedColor = Color.green;
        cb.pressedColor = new Color(0.0f, 0.7f, 0.0f);
        tutorialButton.colors = cb;

        // To re-enable the hover-over highlight.
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(null);
        tutorialTextNormal.text = "Tutorial Enabled";
        tutorialTextPressed.text = "Tutorial Enabled";
        _isTutorial = true;
        Debug.Log("Tutorial has been enabled!");
    }

    void DisableTutorial()
    {
        ColorBlock cb = tutorialButton.colors;
        cb.normalColor = new Color(0.7f, 0.0f, 0.0f);
        cb.selectedColor = cb.normalColor;
        cb.highlightedColor = Color.red;
        cb.pressedColor = new Color(0.7f, 0.0f, 0.0f);
        tutorialButton.colors = cb;

        // To re-enable the hover-over highlight.
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(null);
        tutorialTextNormal.text = "Tutorial Disabled";
        tutorialTextPressed.text = "Tutorial Disabled";
        _isTutorial = false;
        Debug.Log("Tutorial has been disabled!");
    }
}
