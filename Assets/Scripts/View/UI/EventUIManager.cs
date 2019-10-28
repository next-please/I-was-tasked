using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class EventUIManager : MonoBehaviour
{
    public int MaxNumberOfLogs = 3;
    public GameObject Panel;
    public GameObject Log;

    private Queue<GameObject> logs = new Queue<GameObject>();

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
        StartCoroutine(DispatchLog(e.message));
    }

    IEnumerator DispatchLog(String message)
    {
        GameObject newLog = Instantiate(Log);
        newLog.transform.SetParent(Panel.transform);
        newLog.transform.localScale = Vector3.one;
        newLog.transform.SetAsFirstSibling(); // spawn on top of previous log

        TextMeshProUGUI logText = newLog.GetComponentInChildren<TextMeshProUGUI>();
        logText.text = message;

        logs.Enqueue(newLog);

        if (logs.Count > MaxNumberOfLogs)
        {
            StartCoroutine(RemoveOldestLog());
        }

        // remove log after some time
        yield return new WaitForSeconds(3f);
        StartCoroutine(RemoveOldestLog());
    }

    IEnumerator RemoveOldestLog()
    {
        if (logs.Count == 0)
        {
            yield break;
        }
        GameObject firstLog = logs.Dequeue();
        Animator animator = firstLog.GetComponent<Animator>();
        animator.Play("Exit");
        yield return new WaitForSeconds(0.5f);
        Destroy(firstLog);
    }
}

public class GlobalMessageEvent : GameEvent
{
    public string message;
}
