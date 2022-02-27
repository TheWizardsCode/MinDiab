using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WizardsCode.MinDiab.UX
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField, Tooltip("The object that we want the cmaera to follow.")]
        Transform target;

        void LateUpdate()
        {
            transform.position = target.position;
        }

        private void OnValidate()
        {
            if (target)
            {
                transform.position = target.position;
            }
        }
    }
}
