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
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.LogFormat("{0}: Received room list update.", CLASS_NAME);
        if(roomList.Count == 0)
        {
            Debug.LogFormat("{0}: Room list update is empty.", CLASS_NAME);
        }
        foreach (RoomInfo info in roomList)
        {
            Debug.LogFormat("{0}: Room name: {1}", CLASS_NAME, info.Name);
            if (info.RemovedFromList)
            {
                int index = _listings
                    .FindIndex(l => l.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                    return;
                }
            }

            Debug.LogFormat("{0}: Instantiating Room '{1}'", 
                CLASS_NAME, info.Name);
            RoomListing listing = Instantiate(_roomListing, _content);
            if (listing != null)
            {
                listing.SetRoomInfo(info);
                _listings.Add(listing);
            }
        }
    }
}
