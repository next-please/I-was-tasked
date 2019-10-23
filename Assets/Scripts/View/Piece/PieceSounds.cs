using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSounds : MonoBehaviour
{
    public AudioSource attackAudioSource;
    public AudioSource skillAudioSource;

    public void InstantiateSounds(Transform pieceViewParent)
    {
        attackAudioSource = Instantiate(attackAudioSource, pieceViewParent);
        // skillAudioSource = Instantiate(skillAudioSource);
    }

    public void PlayAttackSound()
    {
        attackAudioSource.Play();
    }

    public void PlaySkillSound()
    {
        // skillAudioSource.Play();
    }
}
