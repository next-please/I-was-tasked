using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;

public class SynergyTabMenu : MonoBehaviour
{
    private readonly string CLASS_NAME = "SynergyTabMenu";

    [SerializeField]
    private Transform _content;

    [SerializeField]
    private SynergyInfoPanel _info;

    [SerializeField]
    private SynergyTab _synergyTab;

    private List<SynergyTab> _synergyTabs = new List<SynergyTab>();

    public void IncrementSynergyTab(string synergyName, string synergyDescription, int requirementCount)
    {
        foreach (SynergyTab s in _synergyTabs)
        {
            Debug.LogFormat("{0} : {1}", s.SynergyName, synergyName);
        }
        int index = _synergyTabs.FindIndex(iterTab => iterTab.SynergyName == synergyName);
        if (index != -1)
        {
            _synergyTabs[index].AddCount();
        }
        else
        {
            SynergyTab tab = Instantiate(_synergyTab, _content);
            _synergyTabs.Add(tab);
            tab.Initialise(synergyName, synergyDescription, requirementCount, _info);
            tab.AddCount();
        }
        sortTabs();
    }

    public void DecrementSynergyTab(string synergyName)
    {
        int index = _synergyTabs.FindIndex(iterTab => iterTab.SynergyName == synergyName);
        if (index != -1)
        {
            _synergyTabs[index].DecreaseCount();
        }
        else
        {
            Debug.LogErrorFormat("{0}: Tried to decrement synergy tab count but did not exist", CLASS_NAME);
        }
        sortTabs();
    }

    private void sortTabs()
    {
        List<Transform> children = new List<Transform>();
        for (int i = _content.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = _content.transform.GetChild(i);
            children.Add(child);
            child.SetParent(null);
        }
        children.Sort((Transform t1, Transform t2) => { 
            return t2.gameObject.GetComponent<SynergyTab>().Count - t1.gameObject.GetComponent<SynergyTab>().Count; 
        });
        foreach (Transform child in children)
        {
            child.SetParent(_content.transform);
        }
    }

}
