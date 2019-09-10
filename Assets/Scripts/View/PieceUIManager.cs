using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceUIManager : MonoBehaviour
{
    public Canvas pieceCanvas;
    public Text nameText;
    public Text attackDamageText;

    private Piece selectedPiece;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
        attackDamageText.text = piece.GetAttackDamage().ToString();
    }
}
