using Photon.Realtime;
using Photon.Pun;
using System;
using UnityEngine;
using TMPro;

public class CurrentRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomNameAndDetails;

    [SerializeField]
    private PlayerListingsMenu _playerListingsMenu;

    [SerializeField]
    private LeaveRoomMenu _leaveRoomMenu;
    public LeaveRoomMenu LeaveRoomMenu { get { return _leaveRoomMenu; } }

    private CanvasesManager _canvasesManager;

    public void FirstInitialize(CanvasesManager mg)
    {
        _canvasesManager = mg;
        _playerListingsMenu.FirstInitialize(mg);
        _leaveRoomMenu.FirstInitialize(mg);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        UpdateRoomInfo();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateRoomInfo()
    {
        roomNameAndDetails.text = String.Format("{0} ({1}/{2})", PhotonNetwork.CurrentRoom.Name, PhotonNetwork.PlayerList.Length, PhotonNetwork.CurrentRoom.MaxPlayers);
    }
}
