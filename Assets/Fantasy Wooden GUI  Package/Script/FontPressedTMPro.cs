using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FontPressedTMPro : MonoBehaviour, IPointerEnterHandler
{

    public GameObject NormalBtn;
    public GameObject PressedBtn;
    public Sprite PressedBtnSprite;
    private Sprite releasedBtnSprite;

    public void Awake()
    {
        releasedBtnSprite = GetComponent<Image>().sprite;
    }

    public void OnClickDownBtn()
    {
        if (GetComponent<Button>().interactable)
        {
            NormalBtn.gameObject.SetActive(false);
            PressedBtn.gameObject.SetActive(true);
            GetComponent<Image>().sprite = PressedBtnSprite;
            if (SoundManager.Instance.LobbyButtonClick != null)
            {
                SoundManager.Instance.LobbyButtonClick.Play();
            }
        }
    }
    public void OnClickUpBtn()
    {
        if (GetComponent<Button>().interactable)
        {
            NormalBtn.gameObject.SetActive(true);
            PressedBtn.gameObject.SetActive(false);
            GetComponent<Image>().sprite = releasedBtnSprite;

            // To re-enable the hover-over highlight.
            EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            eventSystem.SetSelectedGameObject(null);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SoundManager.Instance.LobbyButtonHover != null)
        {
            SoundManager.Instance.LobbyButtonHover.Play();
        }
    }
}
