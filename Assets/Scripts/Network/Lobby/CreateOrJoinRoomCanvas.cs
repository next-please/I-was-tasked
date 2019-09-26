using UnityEngine;

public class CreateOrJoinRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private CreateRoomMenu _createRoomMenu;

    [SerializeField]
    private RoomListingsMenu _roomsListingMenu;

    private CanvasesManager _canvasesManager;

    public void FirstInitialize(CanvasesManager mg)
    {
        _canvasesManager = mg;
        _createRoomMenu.FirstInitialize(mg);
        _roomsListingMenu.FirstInitialize(mg);
    }
    
}
