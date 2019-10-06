using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceUIManager : MonoBehaviour
{
    public Canvas pieceCanvas;
    public Text nameText;
    public Text attackDamageText;
    public Text rangeText;
    public Text atkSpeedText;
    public Text rarityText;

    void Start()
    {
        pieceCanvas.enabled = false;
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<SelectPieceEvent>(OnPieceSelected);
        EventManager.Instance.AddListener<DeselectPieceEvent>(OnPieceDeselected);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<SelectPieceEvent>(OnPieceSelected);
        EventManager.Instance.RemoveListener<DeselectPieceEvent>(OnPieceDeselected);
    }

    void OnPieceSelected(SelectPieceEvent e)
    {
        ShowCanvas(e.piece);
    }

    void OnPieceDeselected(DeselectPieceEvent e)
    {
        HideCanvas();
    }

    private void ShowCanvas(Piece piece)
    {
        SetPieceInfo(piece);
        pieceCanvas.enabled = true;
    }

    private void HideCanvas()
    {
        pieceCanvas.enabled = false;
    }

    private void SetPieceInfo(Piece piece)
    {
        nameText.text = piece.GetName();
        attackDamageText.text = string.Format("Damage: {0}", piece.GetAttackDamage());
        rangeText.text = string.Format("Range: {0}", piece.GetAttackRange());
        atkSpeedText.text = string.Format("Atk Speed: {0}", piece.GetAttackSpeed());
        rarityText.text = string.Format("Rarity: {0}", piece.GetRarity());
    }
}
