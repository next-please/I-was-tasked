using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager s_Instance = null; // Singleton

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

    // To-do: handle music?
}
