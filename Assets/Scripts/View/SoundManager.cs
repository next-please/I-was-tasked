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
    private const float EndWaveFadeInDuration = 0.0125f;

    public AudioSource PieceDrop = null;
    public AudioSource PiecePickUp = null;
    public AudioSource PiecePurchase = null;
    public AudioSource PieceTrash = null;
    private const float PieceVolume = 0.625f;

    public AudioSource RoundPreStart = null;
    public AudioSource RoundBattleDuration = null;
    public AudioSource AmbientBackground = null;
    public AudioSource GameSoundTrack = null;

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
            PieceDrop.volume = PieceVolume;
        }

        if (PiecePickUp != null)
        {
            PiecePickUp = Instantiate(PiecePickUp, transform);
            PiecePickUp.volume = PieceVolume;
        }

        if (PiecePurchase != null)
        {
            PiecePurchase = Instantiate(PiecePurchase, transform);
            PiecePurchase.volume = PieceVolume;
        }

        if (PieceTrash != null)
        {
            PieceTrash = Instantiate(PieceTrash, transform);
            PieceTrash.volume = PieceVolume;
        }

        if (RoundPreStart != null)
        {
            RoundPreStart = Instantiate(RoundPreStart, transform);
        }

        if (RoundBattleDuration != null)
        {
            RoundBattleDuration = Instantiate(RoundBattleDuration, transform);
        }

        if (AmbientBackground != null)
        {
            AmbientBackground = Instantiate(AmbientBackground, transform);
            AmbientBackground.loop = true;
            IEnumerator fadeIn = FadeInAudioSource(AmbientBackground, 3.0f, 0.25f);
            StartCoroutine(fadeIn);
        }

        if (GameSoundTrack != null)
        {
            GameSoundTrack = Instantiate(GameSoundTrack, transform);
            GameSoundTrack.loop = true;
            IEnumerator fadeIn = FadeInAudioSource(GameSoundTrack, 3.0f, 0.25f);
            StartCoroutine(fadeIn);
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

    public void PlayRoundPreStartSound()
    {
        if (RoundPreStart != null)
        {
            IEnumerator fadeIn = FadeInAudioSource(RoundPreStart, 1.0f, 0.875f);
            StartCoroutine(fadeIn);
        }
    }

    public void PlayRoundBattleDurationSound(bool play)
    {
        // Currently unused.
        if (play && RoundBattleDuration != null)
        {
            IEnumerator fadeIn = FadeInAudioSource(RoundBattleDuration, 3.0f, 1.0f);
            StartCoroutine(fadeIn);
        }
        else if (!play && RoundBattleDuration != null)
        {
            IEnumerator fadeOut = FadeOutAudioSource(RoundBattleDuration, 10.0f);
            StartCoroutine(fadeOut);
        }
    }

    public void PlayEndWaveSound(bool win)
    {
        if (win && WinWave != null)
        {
            IEnumerator fadeIn = FadeInAudioSource(WinWave, EndWaveFadeInDuration, 0.625f);
            StartCoroutine(fadeIn);
        }
        else if (!win && LoseWave != null)
        {
            IEnumerator fadeIn = FadeInAudioSource(LoseWave, EndWaveFadeInDuration, 0.625f);
            StartCoroutine(fadeIn);
        }
    }

    public void PlayEndGameSound(bool win)
    {
        if (win && WinGame != null)
        {
            IEnumerator fadeIn = FadeInAudioSource(WinGame, EndWaveFadeInDuration, 0.75f);
            StartCoroutine(fadeIn);
        }
        else if (!win && LoseGame != null)
        {
            IEnumerator fadeIn = FadeInAudioSource(LoseGame, EndWaveFadeInDuration, 0.75f);
            StartCoroutine(fadeIn);
        }
    }

    private IEnumerator FadeInAudioSource(AudioSource audioSource, float fadeTimeDuration, float maxVolume)
    {
        audioSource.volume = 0.1f;
        audioSource.Play();
        float startTime = Time.time;
        while (audioSource.volume < maxVolume)
        {
            float t = (Time.time - startTime) / fadeTimeDuration;
            audioSource.volume = Mathf.Lerp(audioSource.volume, maxVolume, t);
            yield return null;
        }
    }

    private IEnumerator FadeOutAudioSource(AudioSource audioSource, float fadeTimeDuration)
    {
        float startTime = Time.time;
        while (audioSource.volume > 0)
        {
            float t = (Time.time - startTime) / fadeTimeDuration;
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0.0f, t);
            yield return null;
        }
        audioSource.Stop();
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
