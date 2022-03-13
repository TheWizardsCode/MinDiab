using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WizardsCode.MinDiab.UI
{
    public abstract class AbstractFeedbackManager : MonoBehaviour
    {
        [SerializeField, Tooltip("A canvas to spawn when UI feedback is required.")]
        internal Canvas m_FeedbackCanvas;

        [SerializeField, Tooltip("The default audio source to be used when a specific source is not specified.")]
        internal AudioSource m_DefaultAudioSource;

        internal void SpawnTextFeedback(float damage)
        {
            // OPTIMIZATION: Pool and don't use GetComponent
            Canvas canvas = Instantiate(m_FeedbackCanvas, transform);
            canvas.GetComponentInChildren<TMP_Text>().text = string.Format("{0:0}", damage);
            Destroy(canvas.gameObject, 2);
        }

        internal void PlayAudio(AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            source.Play();
        }

        
    }
}
