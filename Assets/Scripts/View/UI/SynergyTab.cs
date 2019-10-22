﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SynergyTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private readonly float BLUR_OPACITY_VALUE = 0.5f;
    private readonly float CLEAR_OPACTITY_VALUE = 1f;

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
    [SerializeField]
    private Image _background1;
    [SerializeField]
    private Image _background2;
    [SerializeField]
    private Image _background3;

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

    private string _synergyDescription;
    private int _count;
    public int Count { get { return _count; } }
    private int _requirementCount;
    private SynergyInfoPanel _synergyInfoPanel;

    public void Initialise(string synergyName, string synergyDescription, int requirementCount, SynergyInfoPanel ip)
    {
        _synergyName = synergyName;
        _synergyDescription = synergyDescription;
        _requirementCount = requirementCount;
        _synergyInfoPanel = ip;
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

    public void OnPointerEnter(PointerEventData data)
    {
        _synergyInfoPanel.Show(_synergyName, _synergyDescription, _count, _requirementCount);
    }

    public void OnPointerExit(PointerEventData data)
    {
        _synergyInfoPanel.Hide();
    }

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
        setOpacity(_hex1, BLUR_OPACITY_VALUE);
        setOpacity(_hex2, BLUR_OPACITY_VALUE);
        setOpacity(_hex3, BLUR_OPACITY_VALUE);
        setOpacity(_hex4, BLUR_OPACITY_VALUE);
        setOpacity(_hex5, BLUR_OPACITY_VALUE);
        setOpacity(_hex6, BLUR_OPACITY_VALUE);
        setOpacity(_background1, BLUR_OPACITY_VALUE);
        setOpacity(_background2, BLUR_OPACITY_VALUE);
        setOpacity(_background3, BLUR_OPACITY_VALUE);
        setOpacity(_icon, BLUR_OPACITY_VALUE);

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
                setOpacity(_hex1, CLEAR_OPACTITY_VALUE);
                setOpacity(_hex2, CLEAR_OPACTITY_VALUE);
                setOpacity(_hex3, CLEAR_OPACTITY_VALUE);
                setOpacity(_background1, CLEAR_OPACTITY_VALUE);
                setOpacity(_background2, CLEAR_OPACTITY_VALUE);
                setOpacity(_background3, CLEAR_OPACTITY_VALUE);
                setOpacity(_icon, CLEAR_OPACTITY_VALUE);
                break;
        }
    }

    private void setOpacity(Image image, float opacity)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, opacity);
    }


    private bool isActive()
    {
        return _count > 1;
    }
}