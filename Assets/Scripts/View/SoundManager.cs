using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource WinWave = null;
    public AudioSource LoseWave = null;
    public AudioSource WinGame = null;
    public AudioSource LoseGame = null;

    public Image VolumeHandle;
    public Sprite[] VolumeSprites;

    private void Start()
    {
        InstantiateSounds();
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
