using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TrashCanHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image background;
    public TextMeshProUGUI price;
    private Color initialColor;

    public void Start()
    {
        initialColor = background.color;
    }

    public void OnEnable()
    {
        EventManager.Instance.AddListener<ShowTrashCanEvent>(OnShowTrashCanEvent);
    }

    public void OnDisable()
    {
        EventManager.Instance.RemoveListener<ShowTrashCanEvent>(OnShowTrashCanEvent);
    }

    private void OnShowTrashCanEvent(ShowTrashCanEvent e)
    {
        if (e.piece != null)
        {
            price.text = string.Format("+{0}", e.piece.GetPrice());
        }
        else
        {
            price.text = "";
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        background.color = new Color(0.5f, 0.5f, 0.5f, initialColor.a);
    }

    public void OnPointerExit(PointerEventData data)
    {
        background.color = initialColor;
    }
}
