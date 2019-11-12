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
    private Image _outline;
    [SerializeField]
    private Image _indicator;

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

    public void Reset()
    {
        _count = 0;
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
        float status = (float)_count / (float)_requirementCounts[_requirementPointer];

        _outline.gameObject.SetActive(false);
        setOpacity(_icon, BLUR_OPACITY_VALUE);
        setOpacity(_outline, OUTLINE_OPACITY_MIN_VALUE);
        if (_count >= _minRequirementCount)
        {
            _outline.gameObject.SetActive(true);
            setOpacity(_icon, CLEAR_OPACTITY_VALUE);
            setOpacity(_outline, OUTLINE_OPACITY_MIN_VALUE);
        }
        setPercentageStatus(status);
    }

    private void setPercentageStatus(float status)
    {
        _indicator.fillAmount = status;
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
