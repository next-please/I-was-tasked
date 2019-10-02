using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager s_Instance = null; // Singleton
    private System.Random rngesus = new System.Random();

    // Standard Piece Sounds
    public AudioSource[] pieceSwordHitAudioSources;
    public AudioSource[] pieceArrowHitAudioSources;

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

        while (true)
        {
            int i = rngesus.Next(0, audioSources.Length - 1);
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].Play();
                break;
            }
        }
    }
}
