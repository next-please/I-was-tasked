using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SynergyTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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
    private Image foreground;
    [SerializeField]
    private Image outline;

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
        foreground.color = color;
        setOpacity(foreground, 0.4f);
    }

    private void setIndicatorStatus()
    {
        _hex1.gameObject.SetActive(true);
        _hex2.gameObject.SetActive(true);
        _hex3.gameObject.SetActive(true);
        _hex4.gameObject.SetActive(true);
        _hex5.gameObject.SetActive(true);
        _hex6.gameObject.SetActive(true);
        outline.gameObject.SetActive(false);
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
        setOpacity(outline, OUTLINE_OPACITY_MIN_VALUE);

        switch (_count)
        {
            case 1:
                _hex1.gameObject.SetActive(false);
                break;
            case 2:
                _hex1.gameObject.SetActive(false);
                _hex2.gameObject.SetActive(false);
                break;
            default:
                _hex1.gameObject.SetActive(false);
                _hex2.gameObject.SetActive(false);
                _hex3.gameObject.SetActive(false);
                outline.gameObject.SetActive(true);
                setOpacity(_hex1, CLEAR_OPACTITY_VALUE);
                setOpacity(_hex2, CLEAR_OPACTITY_VALUE);
                setOpacity(_hex3, CLEAR_OPACTITY_VALUE);
                setOpacity(_background1, CLEAR_OPACTITY_VALUE);
                setOpacity(_background2, CLEAR_OPACTITY_VALUE);
                setOpacity(_background3, CLEAR_OPACTITY_VALUE);
                setOpacity(_icon, CLEAR_OPACTITY_VALUE);
                setOpacity(outline, OUTLINE_OPACITY_MIN_VALUE);
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

    private void Update()
    {
        if (outline.IsActive())
        {
            float opacity = Mathf.Lerp(OUTLINE_OPACITY_MIN_VALUE, OUTLINE_OPACITY_MAX_VALUE, Mathf.PingPong(Time.time * 1.0f, 1.0f));
            setOpacity(outline, opacity);
        }
    }
}
