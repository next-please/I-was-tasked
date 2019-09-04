using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    // public GameObject HumanWarriorViewPrefab;
    // public GameObject HumanMageViewPrefab;
    // public GameObject HumanDruidViewPrefab;
    // public GameObject HumanPriestViewPrefab;
    // public GameObject HumanRangerViewPrefab;
    // public GameObject HumanRogueViewPrefab;

    // public GameObject ElfWarriorViewPrefab;
    // public GameObject ElfMageViewPrefab;
    // public GameObject ElfDruidViewPrefab;
    // public GameObject ElfPriestViewPrefab;
    // public GameObject ElfRangerViewPrefab;
    // public GameObject ElfRogueViewPrefab;

    // public GameObject OrcWarriorViewPrefab;
    // public GameObject OrcMageViewPrefab;
    // public GameObject OrcDruidViewPrefab;
    // public GameObject OrcPriestViewPrefab;
    // public GameObject OrcRangerViewPrefab;
    // public GameObject OrcRogueViewPrefab;

    // public GameObject UndeadWarriorViewPrefab;
    // public GameObject UndeadMageViewPrefab;
    // public GameObject UndeadDruidViewPrefab;
    // public GameObject UndeadPriestViewPrefab;
    // public GameObject UndeadRangerViewPrefab;
    // public GameObject UndeadRogueViewPrefab;

    // public GameObject NagaWarriorViewPrefab;
    // public GameObject NagaMageViewPrefab;
    // public GameObject NagaDruidViewPrefab;
    // public GameObject NagaPriestViewPrefab;
    // public GameObject NagaRangerViewPrefab;
    // public GameObject NagaRogueViewPrefab;

    // public GameObject DwarfWarriorViewPrefab;
    // public GameObject DwarfDruidViewPrefab;
    // public GameObject DwarfMageViewPrefab;
    // public GameObject DwarfPriestViewPrefab;
    // public GameObject DwarfRangerViewPrefab;
    // public GameObject DwarfRogueViewPrefab;

    // Temporary
    public GameObject DwarfPrefab;
    public GameObject ElfPrefab;
    public GameObject HumanPrefab;
    public GameObject NagaPrefab;
    public GameObject OrcPrefab;
    public GameObject UndeadPrefab;

    public GameObject FriendlyPieceViewPrefab;
    public GameObject EnemyPieceViewPrefab;

    public GameObject TileViewPrefab;
    public Material White;
    public Material Black;
    public float TileSize = 1;

    private Dictionary<(Enums.Race, Enums.Job), GameObject> PieceViewTypeMap;

    void Awake()
    {
        PieceViewTypeMap = new Dictionary<(Enums.Race, Enums.Job), GameObject>()
        {
            {(Enums.Race.Dwarf, Enums.Job.Druid), DwarfPrefab},
            {(Enums.Race.Dwarf, Enums.Job.Mage), DwarfPrefab},
            {(Enums.Race.Dwarf, Enums.Job.Priest), DwarfPrefab},
            {(Enums.Race.Dwarf, Enums.Job.Ranger), DwarfPrefab},
            {(Enums.Race.Dwarf, Enums.Job.Rogue), DwarfPrefab},
            {(Enums.Race.Dwarf, Enums.Job.Warrior), DwarfPrefab},

            {(Enums.Race.Elf, Enums.Job.Druid), ElfPrefab},
            {(Enums.Race.Elf, Enums.Job.Mage), ElfPrefab},
            {(Enums.Race.Elf, Enums.Job.Priest), ElfPrefab},
            {(Enums.Race.Elf, Enums.Job.Ranger), ElfPrefab},
            {(Enums.Race.Elf, Enums.Job.Rogue), ElfPrefab},
            {(Enums.Race.Elf, Enums.Job.Warrior), ElfPrefab},

            {(Enums.Race.Human, Enums.Job.Druid), HumanPrefab},
            {(Enums.Race.Human, Enums.Job.Mage), HumanPrefab},
            {(Enums.Race.Human, Enums.Job.Priest), HumanPrefab},
            {(Enums.Race.Human, Enums.Job.Ranger), HumanPrefab},
            {(Enums.Race.Human, Enums.Job.Rogue), HumanPrefab},
            {(Enums.Race.Human, Enums.Job.Warrior), HumanPrefab},

            {(Enums.Race.Naga, Enums.Job.Druid), NagaPrefab},
            {(Enums.Race.Naga, Enums.Job.Mage), NagaPrefab},
            {(Enums.Race.Naga, Enums.Job.Priest), NagaPrefab},
            {(Enums.Race.Naga, Enums.Job.Ranger), NagaPrefab},
            {(Enums.Race.Naga, Enums.Job.Rogue), NagaPrefab},
            {(Enums.Race.Naga, Enums.Job.Warrior), NagaPrefab},

            {(Enums.Race.Orc, Enums.Job.Druid), OrcPrefab},
            {(Enums.Race.Orc, Enums.Job.Mage), OrcPrefab},
            {(Enums.Race.Orc, Enums.Job.Priest), OrcPrefab},
            {(Enums.Race.Orc, Enums.Job.Ranger), OrcPrefab},
            {(Enums.Race.Orc, Enums.Job.Rogue), OrcPrefab},
            {(Enums.Race.Orc, Enums.Job.Warrior), OrcPrefab},

            {(Enums.Race.Undead, Enums.Job.Druid), UndeadPrefab},
            {(Enums.Race.Undead, Enums.Job.Mage), UndeadPrefab},
            {(Enums.Race.Undead, Enums.Job.Priest), UndeadPrefab},
            {(Enums.Race.Undead, Enums.Job.Ranger), UndeadPrefab},
            {(Enums.Race.Undead, Enums.Job.Rogue), UndeadPrefab},
            {(Enums.Race.Undead, Enums.Job.Warrior), UndeadPrefab},
        };
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<AddPieceToBoardEvent>(OnPieceAdded);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<AddPieceToBoardEvent>(OnPieceAdded);
    }

    public void OnBoardCreated(Board gameBoard)
    {
        int rows = gameBoard.GetNumRows();
        int cols = gameBoard.GetNumCols();
        bool toggle = false;
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < rows; ++j)
            {
                GameObject tile = Instantiate(TileViewPrefab, new Vector3(i, 0, j) * TileSize, Quaternion.identity);
                TileView tileView = tile.GetComponent<TileView>();
                tileView.TrackTile(gameBoard.GetTile(i, j));

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

        GameObject pieceViewPrefab = piece.IsEnemy()
            ? EnemyPieceViewPrefab
            : GetPiece(piece);
        GameObject pieceObj = Instantiate(pieceViewPrefab, new Vector3(i, 1, j) * TileSize, Quaternion.identity);
        PieceView pieceView = pieceObj.GetComponent<PieceView>();
        pieceView.TrackPiece(piece);
        pieceObj.transform.parent = transform;
    }

    // Temporary method
    private GameObject GetPiece(Piece piece)
    {
        try
        {
            GameObject pieceObj = PieceViewTypeMap[(piece.GetRace(), piece.GetClass())];
            return pieceObj;
        }
        catch (KeyNotFoundException)
        {
            return FriendlyPieceViewPrefab;
        }
    }
}
