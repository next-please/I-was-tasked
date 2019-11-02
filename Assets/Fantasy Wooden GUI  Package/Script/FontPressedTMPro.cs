using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FontPressedTMPro : MonoBehaviour
{

    public GameObject NormalBtn;
    public GameObject PressedBtn;

    public void OnClickDownBtn()
    {

        NormalBtn.gameObject.SetActive(false);
        PressedBtn.gameObject.SetActive(true);

    }
    public void OnClickUpBtn()
    {

        NormalBtn.gameObject.SetActive(true);
        PressedBtn.gameObject.SetActive(false);
    }
}
