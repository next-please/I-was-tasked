using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    private readonly string CLASS_NAME = "RoomListingMenu";

    [SerializeField]
    private Transform _content;

    [SerializeField]
    private RoomListing _roomListing;

    private CanvasesManager _canvasesManager;

    private List<RoomListing> _listings = new List<RoomListing>();

    public void FirstInitialize(CanvasesManager mg)
    {
        _canvasesManager = mg;
    }

    public override void OnJoinedRoom()
    {
        _canvasesManager.CurrentRoomCanvas.Show();
        _content.DestroyChildren();
        _listings.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.LogFormat("{0}: Received room list update.", CLASS_NAME);
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = _listings
                    .FindIndex(l => l.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            else
            {
                int index = _listings.FindIndex(l => l.RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    RoomListing listing = Instantiate(_roomListing, _content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _listings.Add(listing);
                    }
                } else
                {
                    _listings[index].SetRoomInfo(info);
                }
            }

        }
    }
}
