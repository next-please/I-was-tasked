using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource Win_Wave = null;
    public AudioSource Lose_Wave = null;
    public AudioSource Win_Game = null;
    public AudioSource Lose_Game = null;

    private void Start()
    {
        InstantiateSounds();
    }

    public void InstantiateSounds()
    {
        if (Win_Wave != null)
        {
            Win_Wave = Instantiate(Win_Wave, transform);
        }

        if (Lose_Wave != null)
        {
            Lose_Wave = Instantiate(Lose_Wave, transform);
        }

        if (Win_Game != null)
        {
            Win_Game = Instantiate(Win_Game, transform);
        }

        if (Win_Game != null)
        {
            Lose_Game = Instantiate(Lose_Game, transform);
        }
    }

    public void PlayEndWaveSound(bool win)
    {
        if (win && Win_Wave != null)
        {
            Win_Wave.Play();
        }
        else if (!win && Lose_Wave != null)
        {
            Lose_Wave.Play();
        }
    }

    public void PlayEndGameSound(bool win)
    {
        if (win && Win_Game != null)
        {
            Win_Game.Play();
        }
        else if (!win && Lose_Game != null)
        {
            Lose_Game.Play();
        }
    }

    public void SetAudioListenerVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
