using UnityEngine;
using Photon.Pun;

public class CanvasesManager : MonoBehaviour
{
    [SerializeField]
    private ConnectCanvas _connectCanvas;
    public ConnectCanvas ConnectCanvas { get { return _connectCanvas; } }

    [SerializeField]
    private CreateOrJoinRoomCanvas _createOrJoinRoomCanvas;
    public CreateOrJoinRoomCanvas CreateOrJoinRoomCanvas { get { return _createOrJoinRoomCanvas; } }

    [SerializeField]
    private CurrentRoomCanvas _currentRoomCanvas;
    public CurrentRoomCanvas CurrentRoomCanvas { get { return _currentRoomCanvas; } }

    public void OnClick_QuitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        FirstInitialize();
        returnToConnect();
    }

    private void FirstInitialize()
    {
        ConnectCanvas.FirstInitialize(this);
        CreateOrJoinRoomCanvas.FirstInitialize(this);
        CurrentRoomCanvas.FirstInitialize(this);
    }

    private void returnToLobby()
    {
        _connectCanvas.Hide();
        _currentRoomCanvas.Hide();
        _createOrJoinRoomCanvas.Show();
    }

    private void returnToConnect()
    {
        _connectCanvas.Show();
        _currentRoomCanvas.Hide();
        _createOrJoinRoomCanvas.Hide();
    }


}
