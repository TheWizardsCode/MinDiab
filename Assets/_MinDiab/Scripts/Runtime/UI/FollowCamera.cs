using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.UI
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField, Tooltip("The object that the camera should follow.")]
        Transform m_Target;

        void Update()
        {
            transform.position = m_Target.position;
        }
    }
}