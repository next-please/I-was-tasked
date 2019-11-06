using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour
{
    public AudioSource WinWave = null;
    public AudioSource LoseWave = null;
    public AudioSource WinGame = null;
    public AudioSource LoseGame = null;

    public AudioSource PieceDrop = null;
    public AudioSource PiecePickUp = null;
    public AudioSource PiecePurchase = null;
    public AudioSource PieceTrash = null;

    public Image VolumeHandle;
    public Sprite[] VolumeSprites;

    private void Start()
    {
        InstantiateSounds();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener<MoveOnBoardEvent>(OnMovePieceOnBoard);
        EventManager.Instance.AddListener<MoveFromBoardToBenchEvent>(OnMoveFromBoardToBench);
        EventManager.Instance.AddListener<MoveFromBenchToBoardEvent>(OnMoveFromBenchToBoard);
        EventManager.Instance.AddListener<MoveOnBenchEvent>(OnMoveOnBench);
        EventManager.Instance.AddListener<TrashPieceOnBoardEvent>(OnTrashPieceOnBoardEvent);
        EventManager.Instance.AddListener<TrashPieceOnBenchEvent>(OnTrashPieceOnBenchEvent);
        EventManager.Instance.AddListener<ShowTrashCanEvent>(OnPiecePickUp);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<MoveOnBoardEvent>(OnMovePieceOnBoard);
        EventManager.Instance.RemoveListener<MoveFromBoardToBenchEvent>(OnMoveFromBoardToBench);
        EventManager.Instance.RemoveListener<MoveFromBenchToBoardEvent>(OnMoveFromBenchToBoard);
        EventManager.Instance.RemoveListener<MoveOnBenchEvent>(OnMoveOnBench);
        EventManager.Instance.RemoveListener<TrashPieceOnBoardEvent>(OnTrashPieceOnBoardEvent);
        EventManager.Instance.RemoveListener<TrashPieceOnBenchEvent>(OnTrashPieceOnBenchEvent);
        EventManager.Instance.RemoveListener<ShowTrashCanEvent>(OnPiecePickUp);
    }

    public void InstantiateSounds()
    {
        if (WinWave != null)
        {
            WinWave = Instantiate(WinWave, transform);
        }

        if (LoseWave != null)
        {
            LoseWave = Instantiate(LoseWave, transform);
        }

        if (WinGame != null)
        {
            WinGame = Instantiate(WinGame, transform);
        }

        if (WinGame != null)
        {
            LoseGame = Instantiate(LoseGame, transform);
        }

        if (PieceDrop != null)
        {
            PieceDrop = Instantiate(PieceDrop, transform);
        }

        if (PiecePickUp != null)
        {
            PiecePickUp = Instantiate(PiecePickUp, transform);
        }

        if (PiecePurchase != null)
        {
            PiecePurchase = Instantiate(PiecePurchase, transform);
        }

        if (PieceTrash != null)
        {
            PieceTrash = Instantiate(PieceTrash, transform);
        }
    }

    public void PlayPieceSound(string type)
    {
        switch (type)
        {
            case "Drop":
                if (PieceDrop != null)
                {
                    PieceDrop.Play();
                }
                break;
            case "Pick Up":
                if (PiecePickUp != null)
                {
                    PiecePickUp.Play();
                }
                break;
            case "Purchase":
                if (PiecePurchase != null)
                {
                    PiecePurchase.Play();
                }
                break;
            case "Trash":
                if (PieceTrash != null)
                {
                    PieceTrash.Play();
                }
                break;
        }
    }

    public void PlayEndWaveSound(bool win)
    {
        if (win && WinWave != null)
        {
            WinWave.Play();
        }
        else if (!win && LoseWave != null)
        {
            LoseWave.Play();
        }
    }

    public void PlayEndGameSound(bool win)
    {
        if (win && WinGame != null)
        {
            WinGame.Play();
        }
        else if (!win && LoseGame != null)
        {
            LoseGame.Play();
        }
    }

    void OnMovePieceOnBoard(MoveOnBoardEvent e)
    {
        PlayPieceSound("Drop");
    }

    void OnMoveFromBoardToBench(MoveFromBoardToBenchEvent e)
    {
        PlayPieceSound("Drop");
    }

    void OnMoveFromBenchToBoard(MoveFromBenchToBoardEvent e)
    {
        PlayPieceSound("Drop");
    }

    void OnMoveOnBench(MoveOnBenchEvent e)
    {
        PlayPieceSound("Drop");
    }

    void OnTrashPieceOnBoardEvent(TrashPieceOnBoardEvent e)
    {
        PlayPieceSound("Trash");
    }

    void OnTrashPieceOnBenchEvent(TrashPieceOnBenchEvent e)
    {
        PlayPieceSound("Trash");
    }

    void OnPiecePickUp(ShowTrashCanEvent e)
    {
        if (e.showTrashCan)
        {
            PlayPieceSound("Pick Up");
        }
    }

    public void SetAudioListenerVolume(float volume)
    {
        AudioListener.volume = volume;
        if (volume <= 0)
        {
            VolumeHandle.sprite = VolumeSprites[0];
        }
        else
        {
            VolumeHandle.sprite = VolumeSprites[1];
        }
    }
}
