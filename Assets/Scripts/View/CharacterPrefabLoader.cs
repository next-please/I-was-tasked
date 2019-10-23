using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPrefabLoader : MonoBehaviour
{
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

            {(Enums.Race.Human, Enums.Job.Druid), HumanDruidPrefab},
            {(Enums.Race.Human, Enums.Job.Knight), HumanKnightPrefab},
            {(Enums.Race.Human, Enums.Job.Mage), HumanMagePrefab},
            {(Enums.Race.Human, Enums.Job.Priest), HumanPriestPrefab},
            {(Enums.Race.Human, Enums.Job.Rogue), HumanRoguePrefab},

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
    }

    public GameObject GetPrefab(Piece piece)
    {
        //if (piece.IsEnemy())
        //{
        //    return EnemyPrefab;
        //}
        return characterPrefabMap[(piece.GetRace(), piece.GetClass())];
    }
}
