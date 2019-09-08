using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPrefabLoader : MonoBehaviour
{
    public GameObject ElfPrefab;
    public GameObject HumanPrefab;
    public GameObject OrcPrefab;
    public GameObject UndeadPrefab;

    public GameObject EnemyPieceViewPrefab;

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

    private Dictionary<(Enums.Race, Enums.Job), GameObject> characterTypeMap;

    void Awake()
    {
        characterTypeMap = new Dictionary<(Enums.Race, Enums.Job), GameObject>()
        {
            {(Enums.Race.Elf, Enums.Job.Druid), ElfPrefab},
            {(Enums.Race.Elf, Enums.Job.Knight), ElfPrefab},
            {(Enums.Race.Elf, Enums.Job.Mage), ElfPrefab},
            {(Enums.Race.Elf, Enums.Job.Priest), ElfPrefab},
            {(Enums.Race.Elf, Enums.Job.Rogue), ElfPrefab},

            {(Enums.Race.Human, Enums.Job.Druid), HumanPrefab},
            {(Enums.Race.Human, Enums.Job.Knight), HumanPrefab},
            {(Enums.Race.Human, Enums.Job.Mage), HumanPrefab},
            {(Enums.Race.Human, Enums.Job.Priest), HumanPrefab},
            {(Enums.Race.Human, Enums.Job.Rogue), HumanPrefab},

            {(Enums.Race.Orc, Enums.Job.Druid), OrcPrefab},
            {(Enums.Race.Orc, Enums.Job.Knight), OrcPrefab},
            {(Enums.Race.Orc, Enums.Job.Mage), OrcPrefab},
            {(Enums.Race.Orc, Enums.Job.Priest), OrcPrefab},
            {(Enums.Race.Orc, Enums.Job.Rogue), OrcPrefab},

            {(Enums.Race.Undead, Enums.Job.Druid), UndeadPrefab},
            {(Enums.Race.Undead, Enums.Job.Knight), UndeadPrefab},
            {(Enums.Race.Undead, Enums.Job.Mage), UndeadPrefab},
            {(Enums.Race.Undead, Enums.Job.Priest), UndeadPrefab},
            {(Enums.Race.Undead, Enums.Job.Rogue), UndeadPrefab},
        };
    }

    public GameObject GetPrefab(Piece piece)
    {
        return characterTypeMap[(piece.GetRace(), piece.GetClass())];
    }
}
