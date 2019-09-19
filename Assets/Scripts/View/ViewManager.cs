using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public CharacterPrefabLoader characterPrefabLoader;
    public InteractionPrefabLoader interactionPrefabLoader;
    public GameObject FriendlyPieceViewPrefab;
    public GameObject EnemyPieceViewPrefab;
    public GameObject BoardPlayerOne;
    public GameObject BoardPlayerTwo;
    public GameObject BoardPlayerThree;
    public Material White;
    public Material Black;

    static float TileSize = 1;
    static float boardOffset = 20;
    struct BoardDimension
    {
        public int rows;
        public int cols;
        public Vector3 startPos;
    }

    private static BoardDimension[] boardDimensions = new BoardDimension[3];

    void OnEnable()
    {
        EventManager.Instance.AddListener<AddPieceToBoardEvent>(OnPieceAdded);
        EventManager.Instance.AddListener<AddInteractionToProcessEvent>(OnInteractionAdded);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<AddPieceToBoardEvent>(OnPieceAdded);
        EventManager.Instance.RemoveListener<AddInteractionToProcessEvent>(OnInteractionAdded);
    }

    public void OnBoardCreated(Board gameBoard, Player player)
    {
        int rows = gameBoard.GetNumRows();
        int cols = gameBoard.GetNumCols();
        bool toggle = false;
        Vector3 startPos = new Vector3(boardOffset, 0, 0) * (int)player; // board + offset
        boardDimensions[(int)player] = new BoardDimension { rows = rows, cols = cols, startPos = startPos };

        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {
                GameObject tile = GetPlayerGameBoard(player).transform.GetChild(i * rows + j).gameObject;

                TileView tileView = tile.GetComponent<TileView>();
                tileView.TrackTile(gameBoard.GetTile(j, i));

                Renderer rend = tile.GetComponent<Renderer>();
                rend.material = toggle ? White : Black;
                toggle = !toggle;
            }
            toggle = !toggle;
        }
    }

    public void OnPieceAdded(AddPieceToBoardEvent e)
    {
        Piece piece = e.piece;
        int i = e.row;
        int j = e.col;
        Player player = e.player;

        Vector3 startPos = (Vector3.right * e.board.GetNumRows() * TileSize + Vector3.right * 2) * (int)player; // board + offset
        GameObject pieceViewPrefab = piece.IsEnemy() ? EnemyPieceViewPrefab : FriendlyPieceViewPrefab;

        Vector3 tileWorldPos = CalculateTileWorldPosition(e.tile);
        tileWorldPos.y = 0.5f;
        GameObject pieceObj = Instantiate(pieceViewPrefab, tileWorldPos, Quaternion.identity);
        PieceView pieceView = pieceObj.GetComponent<PieceView>();
        pieceView.TrackPiece(piece);
        pieceView.InstantiateModelPrefab(characterPrefabLoader.GetPrefab(piece));
        pieceObj.transform.parent = transform;
    }

    public void OnInteractionAdded(AddInteractionToProcessEvent e)
    {
        Interaction interaction = e.interaction;
        GameObject interactionViewPrefab = interactionPrefabLoader.GetPrefab(interaction.interactionPrefab);
        GameObject interactionObj = Instantiate(interactionViewPrefab, Vector3.zero, Quaternion.identity);
        InteractionView interactionView = interactionObj.GetComponent<InteractionView>();
        interactionView.TrackInteraction(interaction);
        interaction.TrackInteractionView(interactionView);
        interactionObj.transform.parent = transform;
    }

    public static Vector3 CalculateTileWorldPosition(Tile tile)
    {
        Player boardOwner = tile.GetBoard().GetOwner();
        BoardDimension boardDimension = boardDimensions[(int)boardOwner];
        int i = tile.GetRow();
        int j = tile.GetCol();
        return boardDimension.startPos + new Vector3(i, 0, j) * TileSize;
    }

    private GameObject GetPlayerGameBoard(Player player)
    {
        switch (player)
        {
            case Player.Zero:
                return BoardPlayerOne;
            case Player.One:
                return BoardPlayerTwo;
            case Player.Two:
                return BoardPlayerThree;
            default:
                return BoardPlayerOne;
        }
    }
}
