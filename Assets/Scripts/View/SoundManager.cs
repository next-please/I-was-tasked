using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager s_Instance = null; // Singleton

    // Standard Piece Sounds
    public AudioSource[] pieceSwordHitAudioSources;
    public AudioSource[] pieceArrowHitAudioSources;

    private int count = 0;

    public enum PieceSound
    {
        SwordHit,
        ArrowHit
    }

    public static SoundManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;
            }

            if (s_Instance == null)
            {
                var obj = new GameObject("Sound Manager");
                s_Instance = obj.AddComponent<SoundManager>();
            }
            return s_Instance;
        }
    }

    private void OnApplicationQuit()
    {
        s_Instance = null;
    }

    public void SetAudioListenerVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void PlayPieceSound(PieceSound pieceSound)
    {
        AudioSource[] audioSources;
        switch (pieceSound)
        {
            case PieceSound.SwordHit:
                audioSources = pieceSwordHitAudioSources;
                break;
            case PieceSound.ArrowHit:
                audioSources = pieceArrowHitAudioSources;
                break;
            default:
                audioSources = new AudioSource[0];
                break;
        }

        // Try to "rotate" the sounds being played (otherwise it gets repetitive).
        while (audioSources[count % audioSources.Length].isPlaying)
        {
            count++;
            if (count % audioSources.Length == 0)
            {
                count = 0;
            }
        }
        audioSources[count % audioSources.Length].Play();
    }
}
