using System;
using UnityEngine;
using UnityEngine.UI;

public class SynergyTab : MonoBehaviour
{
    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Image _hex1;
    [SerializeField]
    private Image _hex2;
    [SerializeField]
    private Image _hex3;
    [SerializeField]
    private Image _hex4;
    [SerializeField]
    private Image _hex5;
    [SerializeField]
    private Image _hex6;

    public Sprite humanIcon;
    public Sprite orcIcon;
    public Sprite elfIcon;
    public Sprite undeadIcon;
    public Sprite knightIcon;
    public Sprite mageIcon;
    public Sprite druidIcon;
    public Sprite rogueIcon;
    public Sprite priestIcon;

    private string _synergyName;
    public string SynergyName { get { return _synergyName; } }

    private int _count;
    private int _requirementCount;

    private void setIcon()
    {
        Debug.Log("SynergyTab: Setting Icon");
        switch (_synergyName)
        {
            case "Human":
                _icon.sprite = humanIcon;
                break;
            case "Elf":
                _icon.sprite = elfIcon;
                break;
            case "Orc":
                _icon.sprite = orcIcon;
                break;
            case "Undead":
                _icon.sprite = undeadIcon;
                break;
            case "Knight":
                _icon.sprite = knightIcon;
                break;
            case "Mage":
                _icon.sprite = mageIcon;
                break;
            case "Druid":
                _icon.sprite = druidIcon;
                break;
            case "Rogue":
                _icon.sprite = rogueIcon;
                break;
            case "Priest":
                _icon.sprite = priestIcon;
                break;
            default:
                break;
        }
    }

    private void setIndicatorStatus()
    {
        _hex1.gameObject.SetActive(false);
        _hex2.gameObject.SetActive(false);
        _hex3.gameObject.SetActive(false);
        _hex4.gameObject.SetActive(false);
        _hex5.gameObject.SetActive(false);
        _hex6.gameObject.SetActive(false);

        switch (_count)
        {
            case 1:
                _hex1.gameObject.SetActive(true);
                break;
            case 2:
                _hex1.gameObject.SetActive(true);
                _hex2.gameObject.SetActive(true);
                break;
            default:
                _hex1.gameObject.SetActive(true);
                _hex2.gameObject.SetActive(true);
                _hex3.gameObject.SetActive(true);
                break;
        }
    }

    public void Initialise(string synergyName, int requirementCount)
    {
        _synergyName = synergyName;
        _requirementCount = requirementCount;
        setIcon();
    }

    public void AddCount()
    {
        _count++;
        this.gameObject.SetActive(isActive());
        setIndicatorStatus();
    }

    public void DecreaseCount()
    {
        _count--;
        Debug.Assert(_count >= 0);
        this.gameObject.SetActive(isActive());
        setIndicatorStatus();
    }

    private bool isActive()
    {
        return _count > 1;
    }
}
