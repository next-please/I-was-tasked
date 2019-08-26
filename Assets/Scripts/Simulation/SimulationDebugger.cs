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
        sim.gameBoard = new Board(8, 8);
        sim.gameBoard.AddPieceToBoard(lewis_enemy, 7, 7);
        sim.gameBoard.AddPieceToBoard(junkai_enemy, 4, 4);
        sim.gameBoard.AddPieceToBoard(jolyn_player, 1, 2);
        sim.gameBoard.AddPieceToBoard(nicholas_player, 0, 0);
    }
}
