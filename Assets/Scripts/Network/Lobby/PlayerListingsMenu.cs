using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    private readonly string CLASS_NAME = "PlayerListingsMenu";

    [SerializeField]
    private Transform _content;

    [SerializeField]
    private PlayerListing _playerListing;

    [SerializeField]
    private GameObject _startGameButton;

    public Button tutorialButton;
    public TextMeshProUGUI tutorialTextNormal;
    public TextMeshProUGUI tutorialTextPressed;
    private bool _isTutorial;
    private bool _ready;

    private List<PlayerListing> _listings = new List<PlayerListing>();
    private CanvasesManager _canvasesManager;

    public override void OnEnable()
    {
        base.OnEnable();
        _startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        _isTutorial = (bool) PhotonNetwork.CurrentRoom.CustomProperties["isTutorial"];
        tutorialButton.interactable = PhotonNetwork.IsMasterClient;
        RPC_ChangeTutorialState(_isTutorial);
        SetNotReady();
        GetCurrentRoomPlayers();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < _listings.Count; i++)
            Destroy(_listings[i].gameObject);
        _listings.Clear();
    }

    public void FirstInitialize(CanvasesManager mg)
    {
        _canvasesManager = mg;
    }

    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach (KeyValuePair<int, Photon.Realtime.Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Photon.Realtime.Player player)
    {
        int index = _listings.FindIndex(l => l.Player == player);
        base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.All, PhotonNetwork.LocalPlayer, _ready);
        if (index != -1)
        {
            _listings[index].SetPlayerInfo(player, _listings);
        }
        else
        {
            PlayerListing listing = Instantiate(_playerListing, _content);
            if (listing != null)
            {
                listing.playerListingsMenu = this;
                _listings.Add(listing);
                listing.SetPlayerInfo(player, _listings);
            }
        }
    }

    public void OnClick_StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < _listings.Count; i++)
            {
                if (!_listings[i].Ready)
                    return;
            }

            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("Main Scene MP");
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        int index = _listings.FindIndex(l => l.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }

        foreach (PlayerListing p in _listings)
        {
            p.SetPlayerInfo(p.Player, _listings);
        }
    }

    public override void OnJoinedRoom()
    {
    }

    public override void OnLeftRoom()
    {
        _content.DestroyChildren();
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        _canvasesManager.CurrentRoomCanvas.LeaveRoomMenu.OnClick_LeaveRoom();
    }

    public void OnClick_ToggleReady()
    {
        if (_ready)
            SetNotReady();
        else
            SetReady();

        // TODO: optimise - no need to send to all
        base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.All, PhotonNetwork.LocalPlayer, _ready);
    }

    public void SetReady()
    {
        _ready = true;
        Debug.LogFormat("{0}: Set Ready!", CLASS_NAME);
    }

    public void SetNotReady()
    {
        _ready = false;
        Debug.LogFormat("{0}: Set Not Ready.", CLASS_NAME);
    }

    [PunRPC]
    private void RPC_ChangeReadyState(Photon.Realtime.Player player, bool ready)
    {
        int index = _listings.FindIndex(l => l.Player == player);
        if (index != -1)
        {
            _listings[index].ToggleReady(ready);
        }
    }

    [PunRPC]
    private void RPC_ChangeTutorialState(bool isTutorial)
    {
        if (isTutorial)
        {
            EnableTutorial();
        }
        else
        {
            DisableTutorial();
        }
    }

    public void OnClick_Tutorial()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        _isTutorial = !_isTutorial;
        PhotonNetwork.CurrentRoom.CustomProperties["isTutorial"] = _isTutorial;
        base.photonView.RPC("RPC_ChangeTutorialState", RpcTarget.All, _isTutorial);
    }

    void EnableTutorial()
    {
        ColorBlock cb = tutorialButton.colors;
        cb.normalColor = new Color(0.0f, 0.7f, 0.0f);
        cb.selectedColor = cb.normalColor;
        cb.highlightedColor = Color.green;
        cb.pressedColor = new Color(0.0f, 0.7f, 0.0f);
        cb.disabledColor = new Color(0.0f, 0.7f, 0.0f);
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
        cb.disabledColor = new Color(0.7f, 0.0f, 0.0f);
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
