using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PieceUIManager : MonoBehaviour
{
    public Canvas pieceCanvas;

    public TextMeshProUGUI nameText;

    public Image classIcon;
    public Image raceIcon;
    public Sprite[] classIcons;
    public Sprite[] raceIcons;
    public Image foregroundClass;
    public Image foregroundRace;

    public TextMeshProUGUI healthFraction;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI attackRate;
    public TextMeshProUGUI dps;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;

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
        SetSkillInfo(piece.spell);
        SetAttackInfo(piece.GetAttackDamage(), piece.GetAttackSpeed());
        SetRarity(piece.GetRarity());
    }

    private void SetHealthInfo(int currentHitPoints, int maximumHitPoints)
    {
        healthFraction.text = string.Format("{0:0,0} / {1:0,0}", currentHitPoints, maximumHitPoints);
    }

    private void SetClassRaceIcons(Enums.Job job, Enums.Race race)
    {
        Color[] classColors = { Color.green, Color.magenta, Color.cyan, Color.white, Color.grey };
        Color[] raceColors = { Color.blue, Color.yellow, Color.red, Color.black };

        classIcon.sprite = classIcons[(int)job];
        Color classColor = classColors[(int)job];
        classColor.a = 0.4f;
        foregroundClass.color = classColor;

        raceIcon.sprite = raceIcons[(int)race];
        Color raceColor = raceColors[(int)race];
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
        portraitCamera.transform.position = rootCameraPosition.transform.position + new Vector3(3 * (int)job, 0, 5 * (int)race);
    }

    private void SetSkillInfo(Enums.Spell spell)
    {
        skillName.text = "Undone Skill";
        skillDescription.text = "No description";

        if (spell == Enums.Spell.ForestSpirits)
        {
            skillName.text = "Forest Spirits";
            skillDescription.text = string.Format(
                "Release {0} spirits that heal {1} per second.",
                GameLogicManager.Inst.Data.Skills.ForestSpiritsCount,
                GameLogicManager.Inst.Data.Skills.ForestSpiritsHeal
            );
        }
        else if (spell == Enums.Spell.ProtectAlly)
        {
            skillName.text = "Protect Ally";
            skillDescription.text = string.Format("Places a shield on ally that transfers damage taken to the Elf Knight for {0} seconds.", (int)(GameLogicManager.Inst.Data.Skills.ProtectAllyLingerTicks / 50));
        }
        else if (spell == Enums.Spell.MagicMissile || spell == Enums.Spell.InfiniteMagicMissile)
        {
            skillName.text = "Magic Missile";
            skillDescription.text = string.Format("Launches a barrage of {0} magic missles randomly at opponents dealing {1} damage each.", GameLogicManager.Inst.Data.Skills.MagicMissileCount, GameLogicManager.Inst.Data.Skills.MagicMissileDamage);
        }
        else if (spell == Enums.Spell.BlessingOfNature)
        {
            skillName.text = "Blessing of Nature";
            skillDescription.text = string.Format("Buffs an allied unit to have increased damage and maximum hit points.");
        }
        else if (spell == Enums.Spell.ShadowStrike)
        {
            skillName.text = "Shadow Strike";
            skillDescription.text = string.Format("Instantly backstabs the furthest unit away, dealing {0} damage.", GameLogicManager.Inst.Data.Skills.ShadowStrikeDamage);
        }
        else if (spell == Enums.Spell.Shapeshift)
        {
            skillName.text = "Shapeshift";
            skillDescription.text = string.Format("Partially transforms to gain {0}x attack damage and {1} attack speed.", GameLogicManager.Inst.Data.Skills.ShapeShiftMultiplierIncrease, GameLogicManager.Inst.Data.Skills.ShapeShiftDefaultAttackSpeedIncrease);
        }
        else if (spell == Enums.Spell.Charge)
        {
            skillName.text = "Charge";
            skillDescription.text = "Charges at the furthest enemy and strikes it.";
        }
        else if (spell == Enums.Spell.Fireblast)
        {
            skillName.text = "Fireblast";
            skillDescription.text = string.Format("Launches a slow fiery projectile dealing {0} damage.", GameLogicManager.Inst.Data.Skills.FireblastDamage);
        }
        else if (spell == Enums.Spell.GreaterHeal)
        {
            skillName.text = "Greater Heal";
            skillDescription.text = string.Format("Restores {0} hit points to an ally.", GameLogicManager.Inst.Data.Skills.GreaterHealAmount);
        }
        else if (spell == Enums.Spell.CheapShot)
        {
            skillName.text = "Cheap Shot";
            skillDescription.text = string.Format("Stuns an enemy for {0} seconds.", (int) (GameLogicManager.Inst.Data.Skills.CheapShotStunTicks / 50));
        }
        else if (spell == Enums.Spell.Barkskin)
        {
            skillName.text = "Barkskin";
            skillDescription.text = string.Format("Reflects {0}% of damage taken back to attackers.", BarkskinSkill.barkskinDefaultReflectAmount);
        }
        else if (spell == Enums.Spell.Rampage)
        {
            skillName.text = "Rampage";
            skillDescription.text = string.Format("Gains {0}% attack speed and {1}% armour.", GameLogicManager.Inst.Data.Skills.RampageAttackSpeed, GameLogicManager.Inst.Data.Skills.RampageArmourPercentage);
        }
        else if (spell == Enums.Spell.Thunderstorm)
        {
            skillName.text = "Thunder Storm";
            skillDescription.text = string.Format("Summons a cloud that fires {0} lightning bolts in an area, dealing {1} damage each.", GameLogicManager.Inst.Data.Skills.ThunderStormCount, GameLogicManager.Inst.Data.Skills.ThunderStormBoltDamage);
        }
        else if (spell == Enums.Spell.CurseOfAgony)
        {
            skillName.text = "Curse of Agony";
            skillDescription.text = string.Format("Curses an enemy to lose {0} health every time they attack.", GameLogicManager.Inst.Data.Skills.CurseOfAgonyCurseAmount);
        }
        else if (spell == Enums.Spell.Evicerate)
        {
            skillName.text = "Evicerate";
            skillDescription.text = string.Format("Strikes an enemy for {0} damage and causing it to lose {1} damage over time.", GameLogicManager.Inst.Data.Skills.EviscerateInitialDamage, GameLogicManager.Inst.Data.Skills.EviscerateBleedDamage);
        }
        else if (spell == Enums.Spell.Moonfire)
        {
            skillName.text = "Moonfire";
            skillDescription.text = string.Format("Calls a beam of moon light to strike an enemy for {0} damage. Recovers quickly.", GameLogicManager.Inst.Data.Skills.MoonfireDefaultDamage);
        }
        else if (spell == Enums.Spell.Rot)
        {
            skillName.text = "Rot";
            skillDescription.text = string.Format("Self-decomposes to deal {0} damage per second to nearby enemies.", GameLogicManager.Inst.Data.Skills.RotDamage);
        }
        else if (spell == Enums.Spell.FrostArmour)
        {
            skillName.text = "Frost Armour";
            skillDescription.text = string.Format("Casts an icy protection on an ally providing {0}% more armour.", GameLogicManager.Inst.Data.Skills.FrostArmourPercentage);
        }
        else if (spell == Enums.Spell.UnholyAura)
        {
            skillName.text = "Unholy Aura";
            skillDescription.text = string.Format("Unleashes a menacing aura, dealing {0} damage per second to nearby enemies.", GameLogicManager.Inst.Data.Skills.UnholyAuraDamage);
        }
        else if (spell == Enums.Spell.MarkForDeath)
        {
            skillName.text = "Mark For Death";
            skillDescription.text = string.Format("Marks an enemy for death, instantly killing it in {0} seconds.", (int)(GameLogicManager.Inst.Data.Skills.MarkForDeathTicks / 50));
        }
    }
}
