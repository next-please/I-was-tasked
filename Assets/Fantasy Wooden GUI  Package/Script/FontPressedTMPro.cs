using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FontPressedTMPro : MonoBehaviour
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

        NormalBtn.gameObject.SetActive(false);
        PressedBtn.gameObject.SetActive(true);
        GetComponent<Image>().sprite = PressedBtnSprite;

    }
    public void OnClickUpBtn()
    {

        NormalBtn.gameObject.SetActive(true);
        PressedBtn.gameObject.SetActive(false);
        GetComponent<Image>().sprite = releasedBtnSprite;
    }
}
