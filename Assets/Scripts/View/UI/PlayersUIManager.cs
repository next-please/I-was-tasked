using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Com.Nextplease.IWT;

public class PlayersUIManager : MonoBehaviour
{
    public static readonly int marketIndex = 3;
    public GameObject[] PlayerAvatars;

    private List<Animator> playerAvatarAnimators;
    private int prevSelectedAvatar;
    private int currSelectedAvatar;

    void OnEnable()
    {
        EventManager.Instance.AddListener<CameraPanEvent>(OnCameraPan);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<CameraPanEvent>(OnCameraPan);
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
