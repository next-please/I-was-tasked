using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPrefabLoader : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject HorsemanPrefab;

    public GameObject ElfDruidPrefab;
    public GameObject ElfKnightPrefab;
    public GameObject ElfMagePrefab;
    public GameObject ElfPriestPrefab;
    public GameObject ElfRoguePrefab;

    public GameObject HumanDruidPrefab;
    public GameObject HumanKnightPrefab;
    public GameObject HumanMagePrefab;
    public GameObject HumanPriestPrefab;
    public GameObject HumanRoguePrefab;
    public GameObject HumanArcherPrefab;
    public GameObject HumanSpearmanPrefab;

    public GameObject OrcDruidPrefab;
    public GameObject OrcKnightPrefab;
    public GameObject OrcMagePrefab;
    public GameObject OrcPriestPrefab;
    public GameObject OrcRoguePrefab;

    public GameObject UndeadDruidPrefab;
    public GameObject UndeadKnightPrefab;
    public GameObject UndeadMagePrefab;
    public GameObject UndeadPriestPrefab;
    public GameObject UndeadRoguePrefab;

    public GameObject BigOrcKnightPrefab;
    public GameObject SmallOrcKnightPrefab;
    public GameObject SmallOrcMagePrefab;

    private Dictionary<(Enums.Race, Enums.Job), GameObject> characterPrefabMap;
    private Dictionary<(Enums.Race, Enums.Job), GameObject> smallCharacterPrefabMap;
    private Dictionary<(Enums.Race, Enums.Job), GameObject> bigCharacterPrefabMap;

    void Awake()
    {
        characterPrefabMap = new Dictionary<(Enums.Race, Enums.Job), GameObject>()
        {
            {(Enums.Race.Elf, Enums.Job.Druid), ElfDruidPrefab},
            {(Enums.Race.Elf, Enums.Job.Knight), ElfKnightPrefab},
            {(Enums.Race.Elf, Enums.Job.Mage), ElfMagePrefab},
            {(Enums.Race.Elf, Enums.Job.Priest), ElfPriestPrefab},
            {(Enums.Race.Elf, Enums.Job.Rogue), ElfRoguePrefab},

            {(Enums.Race.Human, Enums.Job.Druid), HumanDruidPrefab},
            {(Enums.Race.Human, Enums.Job.Knight), HumanKnightPrefab},
            {(Enums.Race.Human, Enums.Job.Mage), HumanMagePrefab},
            {(Enums.Race.Human, Enums.Job.Priest), HumanPriestPrefab},
            {(Enums.Race.Human, Enums.Job.Rogue), HumanRoguePrefab},
            {(Enums.Race.Human, Enums.Job.Spearman), HumanSpearmanPrefab},
            {(Enums.Race.Human, Enums.Job.Archer), HumanArcherPrefab},

            {(Enums.Race.Orc, Enums.Job.Druid), OrcDruidPrefab},
            {(Enums.Race.Orc, Enums.Job.Knight), OrcKnightPrefab},
            {(Enums.Race.Orc, Enums.Job.Mage), OrcMagePrefab},
            {(Enums.Race.Orc, Enums.Job.Priest), OrcPriestPrefab},
            {(Enums.Race.Orc, Enums.Job.Rogue), OrcRoguePrefab},

            {(Enums.Race.Undead, Enums.Job.Druid), UndeadDruidPrefab},
            {(Enums.Race.Undead, Enums.Job.Knight), UndeadKnightPrefab},
            {(Enums.Race.Undead, Enums.Job.Mage), UndeadMagePrefab},
            {(Enums.Race.Undead, Enums.Job.Priest), UndeadPriestPrefab},
            {(Enums.Race.Undead, Enums.Job.Rogue), UndeadRoguePrefab},
        };
        smallCharacterPrefabMap = new Dictionary<(Enums.Race, Enums.Job), GameObject>()
        {
            {(Enums.Race.Orc, Enums.Job.Knight), SmallOrcKnightPrefab},
            {(Enums.Race.Orc, Enums.Job.Mage), SmallOrcMagePrefab},
        };
        bigCharacterPrefabMap = new Dictionary<(Enums.Race, Enums.Job), GameObject>()
        {
            {(Enums.Race.Orc, Enums.Job.Knight), BigOrcKnightPrefab},
        };

    }

    public GameObject GetPrefab(Piece piece)
    {
        if (piece.GetTitle().Equals("Horseman"))
        {
            return HorsemanPrefab;
        }
        if (piece.size == Enums.Size.Big)
        {
            return bigCharacterPrefabMap[(piece.GetRace(), piece.GetClass())];
        }
        if (piece.size == Enums.Size.Small)
        {
            return smallCharacterPrefabMap[(piece.GetRace(), piece.GetClass())];
        }

        return characterPrefabMap[(piece.GetRace(), piece.GetClass())];
    }
}
