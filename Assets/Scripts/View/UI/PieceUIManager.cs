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

    public TextMeshProUGUI healthFraction;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI attackRate;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI classNameTooltip;
    public TextMeshProUGUI raceNameTooltip;

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
        SetClassRace(piece.GetClass(), piece.GetRace());
        SetSkillInfo(piece.spell);
        SetAttackInfo(piece.GetAttackDamage(), piece.GetAttackSpeed());
        SetRarity(piece.GetRarity());
    }

    private void SetHealthInfo(int currentHitPoints, int maximumHitPoints)
    {
        healthFraction.text = string.Format("{0:0,0} / {1:0,0}", currentHitPoints, maximumHitPoints);
    }

    private void SetClassRace(Enums.Job job, Enums.Race race)
    {
        classIcon.sprite = classIcons[(int)job];
        switch (job)
        {
            case Enums.Job.Druid:
                classNameTooltip.text = "Druid";
                break;
            case Enums.Job.Knight:
                classNameTooltip.text = "Knight";
                break;
            case Enums.Job.Mage:
                classNameTooltip.text = "Mage";
                break;
            case Enums.Job.Priest:
                classNameTooltip.text = "Priest";
                break;
            case Enums.Job.Rogue:
                classNameTooltip.text = "Rogue";
                break;
        }

        raceIcon.sprite = raceIcons[(int)race];
        switch (race)
        {
            case Enums.Race.Elf:
                raceNameTooltip.text = "Elf";
                break;
            case Enums.Race.Human:
                raceNameTooltip.text = "Human";
                break;
            case Enums.Race.Orc:
                raceNameTooltip.text = "Orc";
                break;
            case Enums.Race.Undead:
                raceNameTooltip.text = "Undead";
                break;
        }
    }

    private void SetAttackInfo(int attackDamage, int attackSpeed)
    {
        damage.text = string.Format("{0}", attackDamage);
        attackRate.text = string.Format("{0}", attackSpeed);
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
                "Release {0} spirits that heals all allies for {1} per second.",
                GameLogicManager.Inst.Data.Skills.ForestSpiritsCount,
                GameLogicManager.Inst.Data.Skills.ForestSpiritsHeal
            );
        }
        else if (spell == Enums.Spell.ProtectAlly)
        {
            skillName.text = "Protect Ally";
            skillDescription.text = string.Format("Places a protective shield on an ally that transfers all Damage taken to the Elf Knight for {0} seconds.", (int)(GameLogicManager.Inst.Data.Skills.ProtectAllyLingerTicks / 50));
        }
        else if (spell == Enums.Spell.MagicMissile || spell == Enums.Spell.InfiniteMagicMissile)
        {
            skillName.text = "Magic Missile";
            skillDescription.text = string.Format("Launches a barrage of {0} magic missles at enemies that deals {1} Damage each.", GameLogicManager.Inst.Data.Skills.MagicMissileCount, GameLogicManager.Inst.Data.Skills.MagicMissileDamage);
        }
        else if (spell == Enums.Spell.BlessingOfNature)
        {
            skillName.text = "Blessing of Nature";
            skillDescription.text = string.Format("Temporarily buffs an ally with increased Damage and Maximum HP.");
        }
        else if (spell == Enums.Spell.ShadowStrike)
        {
            skillName.text = "Shadow Strike";
            skillDescription.text = string.Format("Backstabs the furthest enemy away, dealing {0} Damage.", GameLogicManager.Inst.Data.Skills.ShadowStrikeDamage);
        }
        else if (spell == Enums.Spell.Shapeshift)
        {
            skillName.text = "Shapeshift";
            skillDescription.text = string.Format("Partially transforms into a wolf to gain {0}x Attack Damage and {1} Attack Speed.", GameLogicManager.Inst.Data.Skills.ShapeShiftMultiplierIncrease, GameLogicManager.Inst.Data.Skills.ShapeShiftDefaultAttackSpeedIncrease);
        }
        else if (spell == Enums.Spell.Charge)
        {
            skillName.text = "Charge";
            skillDescription.text = string.Format("Charges at the furthest enemy and strikes it for {0} Damage.", GameLogicManager.Inst.Data.Skills.ChargeBaseDamage);
        }
        else if (spell == Enums.Spell.Fireblast)
        {
            skillName.text = "Fireblast";
            skillDescription.text = string.Format("Launches a slow fiery projectile dealing {0} Damage.", GameLogicManager.Inst.Data.Skills.FireblastDamage);
        }
        else if (spell == Enums.Spell.GreaterHeal)
        {
            skillName.text = "Greater Heal";
            skillDescription.text = string.Format("Restores {0} HP to an ally.", GameLogicManager.Inst.Data.Skills.GreaterHealAmount);
        }
        else if (spell == Enums.Spell.CheapShot)
        {
            skillName.text = "Cheap Shot";
            skillDescription.text = string.Format("Stuns an enemy for {0} seconds.", (int) (GameLogicManager.Inst.Data.Skills.CheapShotStunTicks / 50));
        }
        else if (spell == Enums.Spell.Barkskin)
        {
            skillName.text = "Barkskin";
            skillDescription.text = string.Format("Reflects {0}% of Damage taken back to enemy attackers.", BarkskinSkill.barkskinDefaultReflectAmount);
        }
        else if (spell == Enums.Spell.Rampage)
        {
            skillName.text = "Rampage";
            skillDescription.text = string.Format("Gains {0}% Attack Speed and {1}% Armour.", GameLogicManager.Inst.Data.Skills.RampageAttackSpeed, GameLogicManager.Inst.Data.Skills.RampageArmourPercentage);
        }
        else if (spell == Enums.Spell.Thunderstorm)
        {
            skillName.text = "Thunder Storm";
            skillDescription.text = string.Format("Summons a storm cloud that fires {0} lightning bolts in an area, dealing {1} Damage each.", GameLogicManager.Inst.Data.Skills.ThunderStormCount, GameLogicManager.Inst.Data.Skills.ThunderStormBoltDamage);
        }
        else if (spell == Enums.Spell.CurseOfAgony)
        {
            skillName.text = "Curse of Agony";
            skillDescription.text = string.Format("Curses an enemy to lose {0} HP every time they attack.", GameLogicManager.Inst.Data.Skills.CurseOfAgonyCurseAmount);
        }
        else if (spell == Enums.Spell.Evicerate)
        {
            skillName.text = "Evicerate";
            skillDescription.text = string.Format("Strikes an enemy for {0} Damage and cause it to lose {1} HP over time.", GameLogicManager.Inst.Data.Skills.EviscerateInitialDamage, GameLogicManager.Inst.Data.Skills.EviscerateBleedDamage);
        }
        else if (spell == Enums.Spell.Moonbeam)
        {
            skillName.text = "Moonbeam";
            skillDescription.text = string.Format("Calls a beam of moon light to strike an enemy for {0} Damage.", GameLogicManager.Inst.Data.Skills.MoonfireDefaultDamage);
        }
        else if (spell == Enums.Spell.Rot)
        {
            skillName.text = "Rot";
            skillDescription.text = string.Format("Deals {0} damage per second to surrounding enemies.", GameLogicManager.Inst.Data.Skills.RotDamage);
        }
        else if (spell == Enums.Spell.FrostArmour)
        {
            skillName.text = "Frost Armour";
            skillDescription.text = string.Format("Casts an icy protection on an ally that provides {0}% more armour.", GameLogicManager.Inst.Data.Skills.FrostArmourPercentage);
        }
        else if (spell == Enums.Spell.UnholyAura)
        {
            skillName.text = "Unholy Aura";
            skillDescription.text = string.Format("Unleashes a menacing aura that deals {0} damage per second to surrounding enemies.", GameLogicManager.Inst.Data.Skills.UnholyAuraDamage);
        }
        else if (spell == Enums.Spell.MarkForDeath)
        {
            skillName.text = "Mark For Death";
            skillDescription.text = string.Format("Marks an enemy for death, instantly killing it after {0} seconds.", (int)(GameLogicManager.Inst.Data.Skills.MarkForDeathTicks / 50));
        }
    }
}

