using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SynergyTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private readonly float FOREGROUND_BLUR_OPACITY_VALUE = 0.1f;
    private readonly float FOREGROUND_CLEAR_OPACITY_VALUE = 0.2f;
    private readonly float BLUR_OPACITY_VALUE = 0.75f;
    private readonly float CLEAR_OPACTITY_VALUE = 1f;
    private readonly float OUTLINE_OPACITY_MIN_VALUE = 0.1f;
    private readonly float OUTLINE_OPACITY_MAX_VALUE = 0.7f;

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
    [SerializeField]
    private Image _foreground;
    [SerializeField]
    private Image _outline;

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
    private int _minRequirementCount;
    private int _maxRequirementCount;
    private int _requirementPointer;
    private int[] _requirementCounts;
    private SynergyInfoPanel _synergyInfoPanel;

    public void Initialise(string synergyName, string synergyDescription, int[] requirementCounts, SynergyInfoPanel ip)
    {
        _synergyName = synergyName;
        _synergyDescription = synergyDescription;
        _requirementCounts = requirementCounts;
        _requirementPointer = 0;
        _minRequirementCount = requirementCounts[_requirementPointer];
        _maxRequirementCount = requirementCounts[requirementCounts.Length - 1];
        _synergyInfoPanel = ip;
        setIcon();
    }

    public void AddCount()
    {
        _count++;
        if (_count >= _requirementCounts[_requirementPointer] && _requirementPointer < _requirementCounts.Length - 1)
            _requirementPointer++;
        this.gameObject.SetActive(isActive());
        setIndicatorStatus();
    }

    public void DecreaseCount()
    {
        _count--;
        if (_requirementPointer > 0 && _count < _requirementCounts[_requirementPointer - 1])
            _requirementPointer--;
        Debug.Assert(_count >= 0);
        this.gameObject.SetActive(isActive());
        setIndicatorStatus();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        _synergyInfoPanel.Show(_synergyName, _synergyDescription, _count, _requirementCounts[_requirementPointer]);
    }

    public void OnPointerExit(PointerEventData data)
    {
        _synergyInfoPanel.Hide();
    }

    private void setIcon()
    {
        Debug.Log("SynergyTab: Setting Icon");
        Color color = Color.clear;
        switch (_synergyName)
        {
            case "Human":
                _icon.sprite = humanIcon;
                color = Color.blue;
                break;
            case "Elf":
                _icon.sprite = elfIcon;
                color = Color.yellow;
                break;
            case "Orc":
                _icon.sprite = orcIcon;
                color = Color.red;
                break;
            case "Undead":
                _icon.sprite = undeadIcon;
                color = Color.black;
                break;
            case "Knight":
                _icon.sprite = knightIcon;
                color = Color.magenta;
                break;
            case "Mage":
                _icon.sprite = mageIcon;
                color = Color.cyan;
                break;
            case "Druid":
                _icon.sprite = druidIcon;
                color = Color.green;
                break;
            case "Rogue":
                _icon.sprite = rogueIcon;
                color = Color.grey;
                break;
            case "Priest":
                _icon.sprite = priestIcon;
                color = Color.white;
                break;
            default:
                break;
        }
        _foreground.color = color;
    }

    private void setIndicatorStatus()
    {
        int status = (int)(Math.Floor((double)_count / (double)_requirementCounts[_requirementPointer] * 6.0));
        _hex1.gameObject.SetActive(true);
        _hex2.gameObject.SetActive(true);
        _hex3.gameObject.SetActive(true);
        _hex4.gameObject.SetActive(true);
        _hex5.gameObject.SetActive(true);
        _hex6.gameObject.SetActive(true);
        _outline.gameObject.SetActive(false);
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
        setOpacity(_outline, OUTLINE_OPACITY_MIN_VALUE);
        setOpacity(_foreground, FOREGROUND_BLUR_OPACITY_VALUE);
        if (_count >= _minRequirementCount)
        {
            _outline.gameObject.SetActive(true);
            setOpacity(_background1, CLEAR_OPACTITY_VALUE);
            setOpacity(_background2, CLEAR_OPACTITY_VALUE);
            setOpacity(_background3, CLEAR_OPACTITY_VALUE);
            setOpacity(_icon, CLEAR_OPACTITY_VALUE);
            setOpacity(_outline, OUTLINE_OPACITY_MIN_VALUE);
            setOpacity(_foreground, FOREGROUND_CLEAR_OPACITY_VALUE);
        }
        setPercentageStatus(status);
    }

    private void setPercentageStatus(int status)
    {
        switch (status)
        {
            case 0:
                break;
            case 1:
                _hex1.gameObject.SetActive(false);
                if (_count >= _minRequirementCount)
                    setOpacity(_hex1, CLEAR_OPACTITY_VALUE);
                break;
            case 2:
                _hex1.gameObject.SetActive(false);
                _hex2.gameObject.SetActive(false);
                if (_count >= _minRequirementCount)
                {
                    setOpacity(_hex1, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex2, CLEAR_OPACTITY_VALUE);
                }
                break;
            case 3:
                _hex1.gameObject.SetActive(false);
                _hex2.gameObject.SetActive(false);
                _hex3.gameObject.SetActive(false);
                if (_count >= _minRequirementCount)
                {
                    setOpacity(_hex1, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex2, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex3, CLEAR_OPACTITY_VALUE);
                }
                break;
            case 4:
                _hex1.gameObject.SetActive(false);
                _hex2.gameObject.SetActive(false);
                _hex3.gameObject.SetActive(false);
                _hex4.gameObject.SetActive(false);
                if (_count >= _minRequirementCount)
                {
                    setOpacity(_hex1, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex2, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex3, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex4, CLEAR_OPACTITY_VALUE);
                }
                break;
            case 5:
                _hex1.gameObject.SetActive(false);
                _hex2.gameObject.SetActive(false);
                _hex3.gameObject.SetActive(false);
                _hex4.gameObject.SetActive(false);
                _hex5.gameObject.SetActive(false);
                if (_count >= _minRequirementCount)
                {
                    setOpacity(_hex1, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex2, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex3, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex4, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex5, CLEAR_OPACTITY_VALUE);
                }
                break;
            case 6:
                _hex1.gameObject.SetActive(false);
                _hex2.gameObject.SetActive(false);
                _hex3.gameObject.SetActive(false);
                _hex4.gameObject.SetActive(false);
                _hex5.gameObject.SetActive(false);
                _hex6.gameObject.SetActive(false);
                if (_count >= _minRequirementCount)
                {
                    setOpacity(_hex1, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex2, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex3, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex4, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex5, CLEAR_OPACTITY_VALUE);
                    setOpacity(_hex6, CLEAR_OPACTITY_VALUE);
                }
                break;
            default:
                break;
        }
    }

    private void setOpacity(Image image, float opacity)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, opacity);
    }

    private bool isActive()
    {
        return _count > 0;
    }

    private void Update()
    {
        if (_outline.IsActive())
        {
            float opacity = Mathf.Lerp(OUTLINE_OPACITY_MIN_VALUE, OUTLINE_OPACITY_MAX_VALUE, Mathf.PingPong(Time.time * 1.0f, 1.0f));
            setOpacity(_outline, opacity);
        }
    }
}
