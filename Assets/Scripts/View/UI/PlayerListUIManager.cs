using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Com.Nextplease.IWT;

public class PlayerListUIManager : MonoBehaviour
{
    public Canvas PlayerListCanvas;
    public GameObject[] PlayerPanels;

    private List<TextMeshProUGUI> playerNameTexts;
    private List<Image> highlightMarkers; // Todo: temporary indicator, change later

    private string[] playerNames = { "POTATIO", "AU", "GRATIN" };

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
        SetUpComponents();
        SetUpPanels(); // TODO: get real player list
        HighlightPanel((int)RoomManager.GetLocalPlayer());
    }

    void OnCameraPan(CameraPanEvent e)
    {
        RemoveAllHighlights();
        int playerNum = (int)e.targetView != -1 ? (int)e.targetView : (int)RoomManager.GetLocalPlayer();
        HighlightPanel(playerNum);
    }

    private void SetUpComponents()
    {
        playerNameTexts = new List<TextMeshProUGUI>();
        highlightMarkers = new List<Image>();
        foreach (GameObject panel in PlayerPanels)
        {
            playerNameTexts.Add(panel.GetComponentInChildren<TextMeshProUGUI>());
            highlightMarkers.Add(panel.GetComponentsInChildren<Image>()[1]);
        }
    }

    private void SetUpPanels()
    {
        foreach (GameObject panel in PlayerPanels)
        {
            panel.SetActive(false);
        }
        for (int i = 0; i < playerNames.Length; i++)
        {
            PlayerPanels[i].SetActive(true);
            highlightMarkers[i].enabled = false;
            playerNameTexts[i].text = playerNames[i];
        }
    }

    private void HighlightPanel(int playerNum)
    {
        highlightMarkers[playerNum].enabled = true;
    }

    private void RemoveAllHighlights()
    {
        foreach (Image marker in highlightMarkers)
        {
            marker.enabled = false;
        }
    }
}
