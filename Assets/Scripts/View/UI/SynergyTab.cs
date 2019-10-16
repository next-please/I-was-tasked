using System;
using UnityEngine;
using TMPro;

public class SynergyTab : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    private string _synergyName;
    public string SynergyName { get { return _synergyName; } }

    private int _count;
    private int _requirementCount;

    private void Awake()
    {
        if (_text == null)
            Debug.LogError("SynergyTab: text element not set");
    }

    public void SetText()
    {
        _text.SetText(string.Format("{0}: {1}/{2}", _synergyName, _count, _requirementCount));
    }

    public void Initialise(string synergyName, int requirementCount)
    {
        _synergyName = synergyName;
        _requirementCount = requirementCount;
    }

    public void AddCount()
    {
        _count++;
        SetText();
        this.gameObject.SetActive(isActive());
    }

    public void DecreaseCount()
    {
        _count--;
        Debug.Assert(_count >= 0);
        SetText();
        this.gameObject.SetActive(isActive());
    }

    private bool isActive()
    {
        return _count > 0; // TODO: Change to 1 
    }
}
