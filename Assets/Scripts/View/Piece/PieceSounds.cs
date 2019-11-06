using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSounds : MonoBehaviour
{
    public AudioSource attackAudioSource = null;
    public AudioSource skillCastAudioSource = null;
    public AudioSource skillSubCastAudioSource = null;

    public void InstantiateSounds(Transform pieceViewParent)
    {
        if (attackAudioSource != null)
        {
            attackAudioSource = Instantiate(attackAudioSource, pieceViewParent);
        }

        if (skillCastAudioSource != null)
        {
            skillCastAudioSource = Instantiate(skillCastAudioSource, pieceViewParent);
        }

        if (skillSubCastAudioSource != null)
        {
            skillSubCastAudioSource = Instantiate(skillSubCastAudioSource, pieceViewParent);
        }
    }

    public void PlayAttackSound()
    {
        if (attackAudioSource != null)
        {
            attackAudioSource.Play();
        }
    }

    public void PlaySkillCastSound()
    {
        if (skillCastAudioSource != null)
        {
            skillCastAudioSource.Play();
        }
    }

    public void PlaySkillSubCastSound()
    {
        if (skillSubCastAudioSource != null)
        {
            skillSubCastAudioSource.Play();
        }
    }
}
