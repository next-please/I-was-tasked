using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPrefabLoader : MonoBehaviour
{
    public GameObject ElfPrefab;
    public GameObject HumanPrefab;
    public GameObject OrcPrefab;
    public GameObject UndeadPrefab;

    public GameObject EnemyPrefab;

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

    private Dictionary<(Enums.Race, Enums.Job), GameObject> characterPrefabMap;

    void Awake()
    {
        characterPrefabMap = new Dictionary<(Enums.Race, Enums.Job), GameObject>()
        {
            {(Enums.Race.Elf, Enums.Job.Druid), ElfDruidPrefab},
            {(Enums.Race.Elf, Enums.Job.Knight), ElfKnightPrefab},
            {(Enums.Race.Elf, Enums.Job.Mage), ElfMagePrefab},
            {(Enums.Race.Elf, Enums.Job.Priest), ElfPriestPrefab},
            {(Enums.Race.Elf, Enums.Job.Rogue), ElfRoguePrefab},

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
        if (piece.IsEnemy())
        {
            return EnemyPrefab;
        }
        return characterPrefabMap[(piece.GetRace(), piece.GetClass())];
    }
}
