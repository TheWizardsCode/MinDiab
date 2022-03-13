using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WizardsCode.MinDiab.UI
{
    public class FeedbackManager : MonoBehaviour
    {
        [SerializeField, Tooltip("A canvas to spawn when UI feedback is required.")]
        Canvas m_FeedbackCanvas;

        public void SpawnTextFeedback(float value)
        {
            // OPTIMIZATION: Pool and don't use GetComponent
            Canvas canvas = Instantiate(m_FeedbackCanvas, transform);
            canvas.GetComponentInChildren<TMP_Text>().text = string.Format("{0:0}", value);
            Destroy(canvas.gameObject, 2);
        }
    }
}
