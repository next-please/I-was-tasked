using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSettingsUIManager : MonoBehaviour
{
    public Canvas menuSettingsCanvas;
    public SoundManager soundManager;

    private bool visibility;

    private void Start()
    {
        SetCanvasVisibility(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Toggle Menu Settings"))
        {
            SetCanvasVisibility(!visibility);
        }
    }

    private void SetCanvasVisibility(bool visibility)
    {
        menuSettingsCanvas.enabled = visibility;
        this.visibility = visibility;
    }

    public void SetAudioListenerVolume(float volume)
    {
        soundManager.SetAudioListenerVolume(volume);
    }
}
