using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Controller;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.Cinematics
{
    public class PlayerControlRemover : MonoBehaviour
    {
        Scheduler playerScehduler;
        CharacterRoleController playerController;

        private void Start()
        {
            playerController = GetComponent<CharacterRoleController>();
            playerScehduler = GetComponent<Scheduler>();
        }

        void DisableControl()
        {
            Debug.Log("Disable Control");
        }

        void EnableControl()
        {
            Debug.Log("Enable Control");
        }

    }
}
