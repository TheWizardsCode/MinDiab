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
            Instantiate(m_FeedbackCanvas, transform).GetComponentInChildren<TMP_Text>().text = value.ToString();
        }
    }
}
