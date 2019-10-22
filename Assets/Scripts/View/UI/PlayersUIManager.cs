using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Com.Nextplease.IWT;

public class PlayersUIManager : MonoBehaviour
{
    public static readonly int marketIndex = 3;
    public GameObject[] PlayerAvatars;
    public Image HealthBar;
    public MarketManager marketManager;

    private List<Animator> playerAvatarAnimators;
    private int prevSelectedAvatar;
    private int currSelectedAvatar;

    void OnEnable()
    {
        EventManager.Instance.AddListener<CameraPanEvent>(OnCameraPan);
        EventManager.Instance.AddListener<MarketUpdateEvent>(OnMarketUpdate);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<CameraPanEvent>(OnCameraPan);
        EventManager.Instance.RemoveListener<MarketUpdateEvent>(OnMarketUpdate);
    }

    void Update()
    {
        float newFillAmount = (float)marketManager.GetCastleHealth() / marketManager.StartingCastleHealth;
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, newFillAmount, Time.deltaTime * 1f);
    }

    void Awake()
    {
        playerAvatarAnimators = new List<Animator>();
        foreach (GameObject avatar in PlayerAvatars)
        {
            playerAvatarAnimators.Add(avatar.GetComponent<Animator>());
        }
    }

    void OnCameraPan(CameraPanEvent e)
    {
        int playerNum = (int)e.targetView == -1 ? marketIndex : (int)e.targetView;
        prevSelectedAvatar = currSelectedAvatar;
        currSelectedAvatar = playerNum;

        DeselectPreviousAvatar();
        SelectAvatar(playerNum);
    }

    void OnMarketUpdate(MarketUpdateEvent e)
    {
        float newFillAmount = (float)e.readOnlyMarket.GetCastleHealth() / 10;
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, newFillAmount, Time.deltaTime * 1f);
    }

    private void SelectAvatar(int playerNum)
    {
        if (currSelectedAvatar != marketIndex)
        {
            playerAvatarAnimators[playerNum].Play("Select");
        }
    }

    private void DeselectPreviousAvatar()
    {
        if (prevSelectedAvatar != marketIndex)
        {
            playerAvatarAnimators[prevSelectedAvatar].Play("Deselect");
        }
    }
}
