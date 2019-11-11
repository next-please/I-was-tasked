using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySoundtrack : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
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
}
