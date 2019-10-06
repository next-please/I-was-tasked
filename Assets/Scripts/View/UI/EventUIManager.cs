using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EventUIManager : MonoBehaviour
{
    public Text logText;
    public int MaxNumberOfLines;
    public List<string> currentLog = new List<string>();

    void OnEnable()
    {
        EventManager.Instance.AddListener<GlobalMessageEvent>(OnGlobalMessage);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<GlobalMessageEvent>(OnGlobalMessage);
    }

    void OnGlobalMessage(GlobalMessageEvent e)
    {
        currentLog.Add(e.message);
        string logMessage = "";
        for (int i= Math.Max(0, currentLog.Count - MaxNumberOfLines); i< currentLog.Count; i++)
        {
            logMessage += currentLog[i];
            logMessage += "\n";
        }
        logText.text = logMessage;
    }
}

public class GlobalMessageEvent : GameEvent
{
    public string message;
}
