using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Simulator))]
public class SimulationDebugger : MonoBehaviour
{
    void Awake()
    {
        Simulator sim = GetComponent<Simulator>();
        Piece lewis_enemy = new Piece("Lewis the Jesus Koh", 100, 10, 3, true);
        Piece junkai_enemy = new Piece("Jun the Supreme Kai", 100, 20, 1, true);
        Piece jolyn_player = new Piece("Jo Jo Lyn", 100, 25, 1, false);
        Piece nicholas_player = new Piece("Nick Pepega Chua", 100, 30, 4, false);
        sim.CreateBoard(8, 8);
        sim.AddPieceToBoard(lewis_enemy, 7, 7);
        sim.AddPieceToBoard(junkai_enemy, 4, 4);
        sim.AddPieceToBoard(jolyn_player, 1, 2);
        sim.AddPieceToBoard(nicholas_player, 0, 0);
    }
}
