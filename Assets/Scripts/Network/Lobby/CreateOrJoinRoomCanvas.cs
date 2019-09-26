using UnityEngine;

public class CreateOrJoinRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private CreateRoomMenu _createRoomMenu;

    private CanvasesManager _canvasesManager;

    public void FirstInitialize(CanvasesManager mg)
    {
        _canvasesManager = mg;
        _createRoomMenu.FirstInitialize(mg);
    }
    
}
