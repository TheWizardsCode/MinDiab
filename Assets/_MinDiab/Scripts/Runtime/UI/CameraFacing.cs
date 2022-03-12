using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Controller;

namespace WizardsCode.MinDiab.UI
{
    public class CameraFacing : MonoBehaviour
    {
        CharacterRoleController controller;

        private void Awake()
        {
            controller = FindObjectOfType<CharacterRoleController>();
        }

        private void Update()
        {
            transform.forward = controller.MainCamera.transform.forward;
        }
    }
}