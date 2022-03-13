using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WizardsCode.MinDiab.UI
{
    public class WeaponFeedbackManager : AbstractFeedbackManager
    {
        [Header("Sounds")]
        [SerializeField, Tooltip("Audio clips to play, at random, when starting an attack with this weapon.")]
        AudioClip[] m_StartAttackClips;
        [SerializeField, Tooltip("Audio clips to play, at random, when hitting with this weapon.")]
        AudioClip[] m_HitClips;

        public void OnStartAttack()
        {
            if (m_StartAttackClips.Length == 0) return;
            PlayAudio(m_DefaultAudioSource, m_StartAttackClips[Random.Range(0, m_StartAttackClips.Length)]);
        }

        public void OnHit()
        {
            if (m_HitClips.Length == 0) return;

            PlayAudio(m_DefaultAudioSource, m_HitClips[Random.Range(0, m_HitClips.Length)]);
        }
    }
}
