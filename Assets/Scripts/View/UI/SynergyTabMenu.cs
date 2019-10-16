using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;

public class SynergyTabMenu : MonoBehaviour
{
    private readonly string CLASS_NAME = "SynergyTabMenu";

    [SerializeField]
    private Transform _content;

    [SerializeField]
    private SynergyTab _synergyTab;

    private List<SynergyTab> _synergyTabs = new List<SynergyTab>();

    public void IncrementSynergyTab(string synergyName, int requirementCount)
    {
        foreach(SynergyTab s in _synergyTabs) {
            Debug.LogFormat("{0} : {1}", s.SynergyName, synergyName);
        }
        int index = _synergyTabs.FindIndex(iterTab => iterTab.SynergyName == synergyName);
        if(index != -1)
        {
            _synergyTabs[index].AddCount();
        } else
        {
            SynergyTab tab = Instantiate(_synergyTab, _content);
            _synergyTabs.Add(tab);
            tab.Initialise(synergyName, requirementCount);
            tab.AddCount();
        }
    }

    public void DecrementSynergyTab(string synergyName)
    {
        int index = _synergyTabs.FindIndex(iterTab => iterTab.SynergyName == synergyName);
        if (index != -1)
        {
            _synergyTabs[index].DecreaseCount();
        } else
        {
            Debug.LogErrorFormat("{0}: Tried to decrement synergy tab count but did not exist", CLASS_NAME);
        }
    }

}
