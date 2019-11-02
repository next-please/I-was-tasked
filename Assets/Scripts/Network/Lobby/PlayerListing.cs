using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerListing : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private RawImage _readyIndicator;

    private bool _ready = false;
    public bool Ready { get { return _ready; } }

    public Photon.Realtime.Player Player { get; private set; }

    private void Awake()
    {
        _readyIndicator.color = Color.red;
    }

    public void SetPlayerInfo(Photon.Realtime.Player player)
    {
        Player = player;
        _text.text = player.NickName;
    }

    public void ToggleReady(bool state)
    {
        _ready = state;
        if (_ready)
            _readyIndicator.color = Color.green;
        else
            _readyIndicator.color = Color.red;

    }
}
