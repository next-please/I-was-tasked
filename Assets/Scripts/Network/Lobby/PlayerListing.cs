using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Photon.Pun;
using System.Linq;

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

    [SerializeField]
    private GameObject readyButton;

    [SerializeField]
    private TextMeshProUGUI textNormal;

    [SerializeField]
    private TextMeshProUGUI textPressed;

    public PlayerListingsMenu playerListingsMenu { get; set; }
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
        readyButton.SetActive(false);
        readyButton.GetComponent<Image>().color = Color.red;
    }

    public void SetPlayerInfo(Photon.Realtime.Player player, List<PlayerListing> _listing)
    {
        Player = player;
        _text.text = player.NickName;
        //int index = _listing.FindIndex(l => l.Player == player);
        int index = PhotonNetwork.PlayerList.ToList<Photon.Realtime.Player>().FindIndex(l => l == player);
        _avatar.sprite = avatarSprites[index];
        readyButton.SetActive(player == PhotonNetwork.LocalPlayer);
    }

    public void OnClick_ToggleReady()
    {
        playerListingsMenu.OnClick_ToggleReady();
    }

    public void ToggleReady(bool state)
    {
        _ready = state;
        if (_ready)
        {
            readyButton.GetComponent<Image>().color = Color.green;
            _tab.color = Color.green;
            _avatar.CrossFadeAlpha(1.0f, 0.0f, false);
            _text.color = Color.white;
            _status.SetActive(true);
            textNormal.text = "Ready!";
            textPressed.text = "Ready!";
        }
        else
        {
            readyButton.GetComponent<Image>().color = Color.red;
            _tab.color = Color.red;
            _avatar.CrossFadeAlpha(0.3f, 0.0f, false);
            _text.color = new Color(0.5f, 0.0f, 0.0f);
            _status.SetActive(false);
            textNormal.text = "Ready?";
            textPressed.text = "Ready?";
        }
    }
}
