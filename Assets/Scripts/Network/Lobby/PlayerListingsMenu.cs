﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    private readonly string CLASS_NAME = "PlayerListingsMenu";

    [SerializeField]
    private Transform _content;

    [SerializeField]
    private PlayerListing _playerListing;

    [SerializeField]
    private Text _readyText;

    [SerializeField]
    private GameObject _readyButton;

    private Image _readyImage;

    private bool _ready;

    private List<PlayerListing> _listings = new List<PlayerListing>();
    private CanvasesManager _canvasesManager;

    public override void OnEnable()
    {
        base.OnEnable();
        _readyImage = _readyButton.gameObject.GetComponent<Image>();
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
        if (index != -1)
        {
            _listings[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = Instantiate(_playerListing, _content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                _listings.Add(listing);
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
        _readyText.text = "Ready";
        _readyImage.color = Color.green;
        _ready = true;
        Debug.LogFormat("{0}: Set Ready!", CLASS_NAME);
    }

    public void SetNotReady()
    {
        _readyText.text = "Not Ready";
        _readyImage.color = Color.red;
        _ready = false;
        Debug.LogFormat("{0}: Set Not Ready.", CLASS_NAME);

    }

    [PunRPC]
    private void RPC_ChangeReadyState(Photon.Realtime.Player player, bool ready)
    {
        int index = _listings.FindIndex(l => l.Player == player);
        if (index != -1)
            _listings[index].ToggleReady(ready);
    }
}
