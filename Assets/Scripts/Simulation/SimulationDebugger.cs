using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Simulator))]
public class SimulationDebugger : MonoBehaviour
{
    void Awake()
    {
        Simulator sim = GetComponent<Simulator>();
        MeleePiece lewis_enemy = new MeleePiece("Lewis the Jesus Koh", 100, 1, true);
        MeleePiece junkai_enemy = new MeleePiece("Jun the Supreme Kai", 100, 2, true);
        MeleePiece jolyn_player = new MeleePiece("Jo Jo Lyn", 100, 3, false);
        MeleePiece nicholas_player = new MeleePiece("Nick Pepega Chua", 100, 4, false);
        sim.CreateBoard(8, 8);
        sim.AddPieceToBoard(lewis_enemy, 7, 7);
        sim.AddPieceToBoard(junkai_enemy, 4, 4);
        sim.AddPieceToBoard(jolyn_player, 1, 2);
        sim.AddPieceToBoard(nicholas_player, 0, 0);
    }
}
