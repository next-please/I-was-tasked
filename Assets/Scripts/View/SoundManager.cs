using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource WinWave = null;
    public AudioSource LoseWave = null;
    public AudioSource WinGame = null;
    public AudioSource LoseGame = null;
    private const float WinLoseWaveGameVolume = 0.625f;
    private const float QuickFadeTime = 0.0125f;

    public AudioSource PieceDrop = null;
    public AudioSource PiecePickUp = null;
    public AudioSource PiecePurchase = null;
    public AudioSource PieceTrash = null;
    private const float PieceVolume = 0.625f;

    public AudioSource RoundPreStart = null;
    public AudioSource AmbientBackground = null;
    public AudioSource GameSoundTrack = null;
    public AudioSource LobbySoundTrack = null;
    private const float LobbySoundTrackVolume = 0.5f;
    private bool firstLoadLobby = true;

    public Image VolumeHandle;
    public Sprite[] VolumeSprites;

    private static SoundManager instance = null;

    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            InstantiateSounds();
            DontDestroyOnLoad(this.gameObject);
        }
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
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();
        if (scene.name == "Lobby")
        {
            PlayGameplayMusic(false);
            if (firstLoadLobby)
            {
                LobbySoundTrack.Play();
                firstLoadLobby = false;
            }
            else
            {
                IEnumerator fadeInLobbyMusic = FadeInAudioSource(LobbySoundTrack, 3.0f, LobbySoundTrackVolume);
                StartCoroutine(fadeInLobbyMusic);
            }
        }
        else if (scene.name == "Main Scene MP")
        {
            PlayGameplayMusic(true);
            IEnumerator fadeOutLobbyMusic = FadeOutAudioSource(LobbySoundTrack, 5.0f, 0.0f);
            StartCoroutine(fadeOutLobbyMusic);
        }
    }

    public void InstantiateSounds()
    {
        if (WinWave != null)
        {
            WinWave = Instantiate(WinWave, transform);
            WinWave.volume = WinLoseWaveGameVolume;
        }

        if (LoseWave != null)
        {
            LoseWave = Instantiate(LoseWave, transform);
            LoseWave.volume = WinLoseWaveGameVolume;
        }

        if (WinGame != null)
        {
            WinGame = Instantiate(WinGame, transform);
            WinGame.volume = WinLoseWaveGameVolume;
        }

        if (WinGame != null)
        {
            LoseGame = Instantiate(LoseGame, transform);
            LoseGame.volume = WinLoseWaveGameVolume;
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
            RoundPreStart.volume = 1.0f;
        }

        if (AmbientBackground != null)
        {
            AmbientBackground = Instantiate(AmbientBackground, transform);
            AmbientBackground.loop = true;
        }

        if (GameSoundTrack != null)
        {
            GameSoundTrack = Instantiate(GameSoundTrack, transform);
            GameSoundTrack.loop = true;
        }

        if (LobbySoundTrack != null)
        {
            LobbySoundTrack = Instantiate(LobbySoundTrack, transform);
            LobbySoundTrack.loop = true;
            LobbySoundTrack.volume = LobbySoundTrackVolume;
        }
    }

    private void PlayGameplayMusic(bool play)
    {
        if (play)
        {
            IEnumerator fadeInAmbientBackground = FadeInAudioSource(AmbientBackground, 3.0f, 0.25f);
            StartCoroutine(fadeInAmbientBackground);
            IEnumerator fadeInGameSoundTrack = FadeInAudioSource(GameSoundTrack, 3.0f, 0.25f);
            StartCoroutine(fadeInGameSoundTrack);
        }
        else
        {
            IEnumerator fadeOutAmbientBackground = FadeOutAudioSource(AmbientBackground, 3.0f, 0.0f);
            StartCoroutine(fadeOutAmbientBackground);
            IEnumerator fadeOutGameSoundTrack = FadeOutAudioSource(GameSoundTrack, 3.0f, 0.0f);
            StartCoroutine(fadeOutGameSoundTrack);
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
        IEnumerator fadeIn = FadeInAudioSource(RoundPreStart, QuickFadeTime, 1.0f);
        StartCoroutine(fadeIn);
        IEnumerator fadeOutAndIn = FadeOutAndInAudioSource(GameSoundTrack, RoundPreStart.clip.length, 0.05f);
        StartCoroutine(fadeOutAndIn);
    }

    public void PlayEndWaveSound(bool win)
    {
        IEnumerator fadeOutAndIn = null;
        if (win && WinWave != null)
        {
            WinWave.Play();
            fadeOutAndIn = FadeOutAndInAudioSource(GameSoundTrack, Mathf.Floor(WinWave.clip.length), 0.1f);
            
        }
        else if (!win && LoseWave != null)
        {
            LoseWave.Play();
            fadeOutAndIn = FadeOutAndInAudioSource(GameSoundTrack, Mathf.Floor(LoseWave.clip.length), 0.1f);
        }
        StartCoroutine(fadeOutAndIn);
    }

    public void PlayEndGameSound(bool win)
    {
        IEnumerator fadeOutAndIn = null;
        if (win && WinGame != null)
        {
            WinGame.Play();
            fadeOutAndIn = FadeOutAndInAudioSource(GameSoundTrack, Mathf.Floor(WinGame.clip.length), 0.1f);
        }
        else if (!win && LoseGame != null)
        {
            LoseGame.Play();
            fadeOutAndIn = FadeOutAndInAudioSource(GameSoundTrack, Mathf.Floor(LoseGame.clip.length), 0.1f);

        }
        StartCoroutine(fadeOutAndIn);
    }

    private IEnumerator FadeInAudioSource(AudioSource audioSource, float fadeTimeDuration, float maxVolume)
    {
        if (audioSource == null)
        {
            yield break;
        }

        audioSource.volume = 0.1f;
        audioSource.Play();
        float startTime = Time.time;
        while (audioSource.volume < maxVolume)
        {
            float t = (Time.time - startTime) / fadeTimeDuration;
            audioSource.volume = Mathf.SmoothStep(audioSource.volume, maxVolume, t);
            yield return null;
        }
    }

    private IEnumerator FadeOutAudioSource(AudioSource audioSource, float fadeTimeDuration, float minVolume)
    {
        if (audioSource == null)
        {
            yield break;
        }

        float startTime = Time.time;
        while (audioSource.volume > minVolume)
        {
            float t = (Time.time - startTime) / fadeTimeDuration;
            audioSource.volume = Mathf.SmoothStep(audioSource.volume, minVolume, t);
            yield return null;
        }
        audioSource.Stop();
    }

    private IEnumerator FadeOutAndInAudioSource(AudioSource audioSource, float fadeTimeDuration, float minVolume)
    {
        if (audioSource == null)
        {
            yield break;
        }

        float initialVolume = audioSource.volume;
        float timeElapsed = 0.0f;
        while (timeElapsed < fadeTimeDuration)
        {
            float timeFraction = timeElapsed / fadeTimeDuration;
            float newVolume = initialVolume * Mathf.Cos(timeFraction * 2.0f * Mathf.PI);
            audioSource.volume = (newVolume > minVolume) ? newVolume : minVolume;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = initialVolume;
    }

    private void OnMovePieceOnBoard(MoveOnBoardEvent e)
    {
        PlayPieceSound("Drop");
    }

    private void OnMoveFromBoardToBench(MoveFromBoardToBenchEvent e)
    {
        PlayPieceSound("Drop");
    }

    private void OnMoveFromBenchToBoard(MoveFromBenchToBoardEvent e)
    {
        PlayPieceSound("Drop");
    }

    private void OnMoveOnBench(MoveOnBenchEvent e)
    {
        PlayPieceSound("Drop");
    }

    private void OnTrashPieceOnBoardEvent(TrashPieceOnBoardEvent e)
    {
        PlayPieceSound("Trash");
    }

    private void OnTrashPieceOnBenchEvent(TrashPieceOnBenchEvent e)
    {
        PlayPieceSound("Trash");
    }

    private void OnPiecePickUp(ShowTrashCanEvent e)
    {
        if (e.showTrashCan)
        {
            PlayPieceSound("Pick Up");
        }
    }

    public void SetAudioListenerVolume(float volume)
    {
        AudioListener.volume = volume;
        if (VolumeHandle == null)
        {
            VolumeHandle = GameObject.Find("Volume Handle").GetComponent<Image>();
        }

        if (VolumeHandle != null && volume <= 0)
        {
            VolumeHandle.sprite = VolumeSprites[0];
        }
        else if (VolumeHandle != null && volume > 0)
        {
            VolumeHandle.sprite = VolumeSprites[1];
        }
    }
}
