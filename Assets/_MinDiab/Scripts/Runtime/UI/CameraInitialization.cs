using Cinemachine;
using UnityEngine;
using WizardsCode.MinDiab.Controller;

namespace WizardsCode.MinDiab.UI
{
    public class CameraInitialization : MonoBehaviour
    {
        public string lookAtTransformName = "Camera Look At Target";

        private void Awake()
        {
            CharacterRoleController player = GameObject.FindObjectOfType<CharacterRoleController>();
            CinemachineVirtualCameraBase vCamera = GetComponent<CinemachineVirtualCameraBase>();
            vCamera.Follow = player.transform;
            Transform lookAt = player.transform.Find(lookAtTransformName);
            if (lookAt)
            {
                vCamera.LookAt = lookAt;
            }
            else
            {
                vCamera.LookAt = player.transform;
            }

            Destroy(this);
        }
    }
}