using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WizardsCode.MinDiab.UI
{
    public class CharacterFeedbackManager : AbstractFeedbackManager
    {
        [Header("Sounds")]
        [SerializeField, Tooltip("Audio clips to play, at random, when taking damage.")]
        AudioClip[] m_TakeDamageClips;
        [SerializeField, Tooltip("Audio clips to play, at random, when taking dieing.")]
        AudioClip[] m_DieClips;

        public void OnTakeDamage(float damage)
        {
            SpawnTextFeedback(damage);
            PlayAudio(m_DefaultAudioSource, m_TakeDamageClips[Random.Range(0, m_TakeDamageClips.Length)]);
        }

        public void OnDie()
        {
            PlayAudio(m_DefaultAudioSource, m_DieClips[Random.Range(0, m_DieClips.Length)]);
        }
    }
}
