using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerListing : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private Sprite[] avatarSprites;

    [SerializeField]
    private Image _avatar;

    [SerializeField]
    private GameObject _status;

    private RawImage _tab;

    private bool _ready = false;
    public bool Ready { get { return _ready; } }

    public Photon.Realtime.Player Player { get; private set; }

    private void Awake()
    {
        _tab = GetComponent<RawImage>();
        _tab.color = Color.red;
        _text.color = new Color(0.5f, 0.0f, 0.0f);
        _avatar.CrossFadeAlpha(0.3f, 0.0f, false);
        _status.SetActive(false);
    }

    public void SetPlayerInfo(Photon.Realtime.Player player)
    {
        Player = player;
        _text.text = player.NickName;
        int index = (player.ActorNumber >= 1) ? ((player.ActorNumber % 3) - 1) : 0;
        _avatar.sprite = avatarSprites[index];
    }

    public void ToggleReady(bool state)
    {
        _ready = state;
        if (_ready)
        {
            _tab.color = Color.green;
            _avatar.CrossFadeAlpha(1.0f, 0.0f, false);
            _text.color = Color.white;
            _status.SetActive(true);
        }
        else
        {
            _tab.color = Color.red;
            _avatar.CrossFadeAlpha(0.3f, 0.0f, false);
            _text.color = new Color(0.5f, 0.0f, 0.0f);
            _status.SetActive(false);
        }
    }
}
