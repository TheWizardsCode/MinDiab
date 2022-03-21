using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.UI
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField, Tooltip("The object that the camera should follow.")]
        Transform m_Target;
        [SerializeField, Tooltip("The offset of the camera from the target.")]
        Vector3 m_Offset = new Vector3(0, 10, -7);
        [SerializeField, Tooltip("The speed at which the camera can be moved with in the horizontal axis.")]
        float m_YawSpeed = 100f;
        [SerializeField, Tooltip("The pitch of the camera relative to the target.")]
        float m_Pitch = 2;
        [SerializeField, Tooltip("The speed at which the scroll wheel will change the zoom.")]
        float m_ZoomSpeed = 4;
        [SerializeField, Tooltip("The minimum zoom level allowed for this camera.")]
        float m_MinZoom = 5;
        [SerializeField, Tooltip("The maximum zoom level allowed for this camera.")]
        float m_MaxZoom = 15;

        private float m_CurrentZoom = 10;
        
        float m_CurrentYaw = 0;

        void Update()
        {
            m_CurrentYaw -= Input.GetAxis("Horizontal") * m_YawSpeed * Time.deltaTime;

            m_CurrentZoom -= Input.GetAxis("Mouse ScrollWheel") * m_ZoomSpeed;
            m_CurrentZoom = Mathf.Clamp(m_CurrentZoom, m_MinZoom, m_MaxZoom);
        }

        void LateUpdate()
        {
            transform.position = m_Target.position - m_Offset * m_CurrentZoom;
            transform.LookAt(m_Target.position + Vector3.up * m_Pitch);

            transform.RotateAround(m_Target.position, Vector3.up, m_CurrentYaw);
        }
    }
}