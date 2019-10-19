using UnityEngine;
using UnityEngine.UI;

public class PieceUIManager : MonoBehaviour
{
    public Canvas pieceCanvas;
    public Sprite[] classIcons;
    public Sprite[] raceIcons;
    public Sprite[] meleeRangeIcons;
    public Image classIcon;
    public Image raceIcon;
    public Text nameText;
    public Text skillName;
    public Text skillDescription;
    public Text attackDamage;
    public Text currentHP;
    public Text currentMP; // Might not be an imporant value.
    public Image MeleeRangeIcon;
    public GameObject[] rarityStars;
    private int previousRarityIndex = 0;

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
        currentHP.text = string.Format("{0}", piece.GetMaximumHitPoints());
        currentMP.text = string.Format("{0}", piece.GetMaximumManaPoints());
        SetClassRaceIcons(piece.GetClass(), piece.GetRace());
        SetSkillInfo(piece.GetClass(), piece.GetRace());
        SetAttackInfo(piece.GetAttackRange(), piece.GetAttackDamage());
        SetRarity(piece.GetRarity());
    }

    private void SetClassRaceIcons(Enums.Job job, Enums.Race race)
    {
        classIcon.sprite = classIcons[(int) job];
        raceIcon.sprite = raceIcons[(int) race];
    }

    private void SetAttackInfo(int range, int attackDamage)
    {
        MeleeRangeIcon.sprite = meleeRangeIcons[(range == 1) ? 0 : 1];
        this.attackDamage.text = string.Format("{0}", attackDamage);
        // Unused: rangeText.text = string.Format("Range: {0}", piece.GetAttackRange());
        // Unused: atkSpeedText.text = string.Format("Atk Speed: {0}", piece.GetAttackSpeed());
    }

    private void SetRarity(int rarity)
    {
        if (previousRarityIndex != rarity - 1)
        {
            rarityStars[previousRarityIndex].SetActive(false);
            rarityStars[rarity - 1].SetActive(true);
            previousRarityIndex = rarity - 1;
        }
    }

    private void SetSkillInfo(Enums.Job job, Enums.Race race)
    {
        skillName.text = "Undone Skill";
        skillDescription.text = "No description";

        if (race == Enums.Race.Human && job == Enums.Job.Druid)
        {
            skillName.text = "Shapeshift";
            skillDescription.text = "FURRIES AYAYA Clap";
        }
        else if (race == Enums.Race.Elf && job == Enums.Job.Knight)
        {
            skillName.text = "Protect Ally";
            skillDescription.text = "Squire, attend me!";
        }
        else if (race == Enums.Race.Orc && job == Enums.Job.Druid)
        {
            skillName.text = "Barkskin";
            skillDescription.text = "Woof Woof";
        }
        else if (race == Enums.Race.Elf && job == Enums.Job.Priest)
        {
            skillName.text = "Blessing of Nature";
            skillDescription.text = "Come get healing!";
        }
        else if (race == Enums.Race.Orc && job == Enums.Job.Knight)
        {
            skillName.text = "Rampage";
            skillDescription.text = "ARHHHHHHHHHHHHH";
        }
        else if (race == Enums.Race.Elf && job == Enums.Job.Rogue)
        {
            skillName.text = "Marked for Death";
            skillDescription.text = "This guy's toast";
        }
        else if (race == Enums.Race.Undead && job == Enums.Job.Mage)
        {
            skillName.text = "Frost Armour";
            skillDescription.text = "So cold";
        }
        else if (race == Enums.Race.Orc && job == Enums.Job.Priest)
        {
            skillName.text = "Curse of Agony";
            skillDescription.text = "yOu ArE cUrSeD";
        }
        else if (race == Enums.Race.Orc && job == Enums.Job.Rogue)
        {
            skillName.text = "Evicerate";
            skillDescription.text = "sn1p sn4p";
        }
        else if (race == Enums.Race.Human && job == Enums.Job.Mage)
        {
            skillName.text = "Fireblast";
            skillDescription.text = "Hello";
        }
        else if (race == Enums.Race.Elf && job == Enums.Job.Mage)
        {
            skillName.text = "Magic Missile";
            skillDescription.text = "Pew pew pew";
        }
    }
}
