using UnityEngine;
using UnityEngine.UI;
using System;

public class PieceUIManager : MonoBehaviour
{
    public Canvas pieceCanvas;

    public Text nameText;

    public Image classIcon;
    public Image raceIcon;
    public Sprite[] classIcons;
    public Sprite[] raceIcons;
    public Image foregroundClass;
    public Image foregroundRace;

    public Text healthFraction;
    public Text damage;
    public Text attackRate;
    public Text dps;
    public Text skillName;
    public Text skillDescription;

    public GameObject[] rarityStars;
    private int previousRarityIndex = 0;

    public Camera portraitCamera;
    public Transform rootCameraPosition;

    private Piece currentPiece;
    private bool isSelected = false;

    void Start()
    {
        pieceCanvas.enabled = false;
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<SelectPieceEvent>(OnPieceSelected);
        EventManager.Instance.AddListener<DeselectPieceEvent>(OnPieceDeselected);
        EventManager.Instance.AddListener<HoverPieceEvent>(OnHoverPiece);
        EventManager.Instance.AddListener<PieceHealthChangeEvent>(OnPieceHealthChange);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<SelectPieceEvent>(OnPieceSelected);
        EventManager.Instance.RemoveListener<DeselectPieceEvent>(OnPieceDeselected);
        EventManager.Instance.RemoveListener<HoverPieceEvent>(OnHoverPiece);
        EventManager.Instance.RemoveListener<PieceHealthChangeEvent>(OnPieceHealthChange);
    }

    void OnPieceSelected(SelectPieceEvent e)
    {
        currentPiece = e.piece;
        ShowCanvas(e.piece);
        isSelected = true;
    }

    void OnPieceDeselected(DeselectPieceEvent e)
    {
        currentPiece = null;
        HideCanvas();
        isSelected = false;
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

    private void OnHoverPiece(HoverPieceEvent e)
    {
        // Don't change the card if there's a Piece that is selected.
        if (isSelected)
        {
            return;
        }

        if (e.piece != null)
        {
            currentPiece = e.piece;
            ShowCanvas(e.piece);
        }
        else
        {
            currentPiece = null;
            HideCanvas();
        }
    }

    private void OnPieceHealthChange(PieceHealthChangeEvent e)
    {
        if (currentPiece == e.piece)
        {
            SetHealthInfo(e.piece.GetCurrentHitPoints(), e.piece.GetMaximumHitPoints());
        }
    }

    private void SetPieceInfo(Piece piece)
    {
        nameText.text = piece.GetName();
        SetHealthInfo(piece.GetCurrentHitPoints(), piece.GetMaximumHitPoints());
        SetPortraitCamera(piece.GetClass(), piece.GetRace());
        SetClassRaceIcons(piece.GetClass(), piece.GetRace());
        SetSkillInfo(piece.GetClass(), piece.GetRace());
        SetAttackInfo(piece.GetAttackDamage(), piece.GetAttackSpeed());
        SetRarity(piece.GetRarity());
    }

    private void SetHealthInfo(int currentHitPoints, int maximumHitPoints) {
        healthFraction.text = string.Format("{0:0,0} / {1:0,0}", currentHitPoints, maximumHitPoints);
    }

    private void SetClassRaceIcons(Enums.Job job, Enums.Race race)
    {
        Color[] classColors = { Color.green, Color.magenta, Color.cyan, Color.white, Color.grey };
        Color[] raceColors = { Color.blue, Color.yellow, Color.red, Color.black };

        classIcon.sprite = classIcons[(int) job];
        Color classColor = classColors[(int) job];
        classColor.a = 0.4f;
        foregroundClass.color = classColor;

        raceIcon.sprite = raceIcons[(int) race];
        Color raceColor = raceColors[(int) race];
        raceColor.a = 0.4f;
        foregroundRace.color = raceColor;
    }

    private void SetAttackInfo(int attackDamage, int attackSpeed)
    {
        damage.text = string.Format("{0}", attackDamage);
        attackRate.text = string.Format("{0}", attackSpeed);
        dps.text = string.Format("{0:0.00}", attackDamage * (attackSpeed / 10.0f) * (50.0f / 250.0f));
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

    private void SetPortraitCamera(Enums.Job job, Enums.Race race)
    {
        portraitCamera.transform.position = rootCameraPosition.transform.position + new Vector3(3 * (int) job, 0, 5 * (int) race);
    }

    private void SetSkillInfo(Enums.Job job, Enums.Race race)
    {
        skillName.text = "Undone Skill";
        skillDescription.text = "No description";

        if (race == Enums.Race.Elf)
        {
            if (job == Enums.Job.Druid)
            {
                skillName.text = "Forest Spirits";
                skillDescription.text = "Come get healing!";
            }
            else if (job == Enums.Job.Knight)
            {
                skillName.text = "Protect Ally";
                skillDescription.text = "Squire, attend me! Ready sir! gachiBASS Clap";
            }
            else if (job == Enums.Job.Mage)
            {
                skillName.text = "Magic Missile";
                skillDescription.text = "Pew pew pew! Totally not from Warcraft.";
            }
            else if (job == Enums.Job.Priest)
            {
                skillName.text = "Blessing of Nature";
                skillDescription.text = "Two's a Pair, Tree's a Crowd.";
            }
            else if (job == Enums.Job.Rogue)
            {
                skillName.text = "Marked for Death";
                skillDescription.text = "Heh, this guy's toast!";
            }
        }
        else if (race == Enums.Race.Human)
        {
            if (job == Enums.Job.Druid)
            {
                skillName.text = "Shapeshift";
                skillDescription.text = "FURRIES AYAYA Clap";
            }
            else if (job == Enums.Job.Knight)
            {
                skillName.text = "Charge";
                skillDescription.text = "勧め!";
            }
            else if (job == Enums.Job.Mage)
            {
                skillName.text = "Fireblast";
                skillDescription.text = "Hello.";
            }
            else if (job == Enums.Job.Priest)
            {
                skillName.text = "Greater Heal";
                skillDescription.text = "You are purified!";
            }
            else if (job == Enums.Job.Rogue)
            {
                skillName.text = "Cheap Shot";
                skillDescription.text = "Only costs Tree Fiddy.";
            }
        }
        else if (race == Enums.Race.Orc)
        {
            if (job == Enums.Job.Druid)
            {
                skillName.text = "Barkskin";
                skillDescription.text = "Woof Woof";
            }
            else if (job == Enums.Job.Knight)
            {
                skillName.text = "Rampage";
                skillDescription.text = "ARHHHHHHHHHHHHH";
            }
            else if (job == Enums.Job.Mage)
            {
                skillName.text = "Thunder Storm";
                skillDescription.text = "The Sounds of Thunder will Electrify You, I guess.";
            }
            else if (job == Enums.Job.Priest)
            {
                skillName.text = "Curse of Agony";
                skillDescription.text = "yOu ArE cUrSeD";
            }
            else if (job == Enums.Job.Rogue)
            {
                skillName.text = "Evicerate";
                skillDescription.text = "sn1p sn4p";
            }
        }
        else if (race == Enums.Race.Undead)
        {
            if (job == Enums.Job.Druid)
            {
                skillName.text = "Moonfire";
                skillDescription.text = "Best results on a Full Moon.";
            }
            else if (job == Enums.Job.Knight)
            {
                skillName.text = "Rot";
                skillDescription.text = "Frostmourne? Who needs that?";
            }
            else if (job == Enums.Job.Mage)
            {
                skillName.text = "Frost Armour";
                skillDescription.text = "So cold.";
            }
            else if (job == Enums.Job.Priest)
            {
                skillName.text = "Unholy Aura";
                skillDescription.text = "For immoral people only.";
            }
            else if (job == Enums.Job.Rogue)
            {
                skillName.text = "Mark For Death";
                skillDescription.text = "That living creature will join us!";
            }
        }
    }
}
