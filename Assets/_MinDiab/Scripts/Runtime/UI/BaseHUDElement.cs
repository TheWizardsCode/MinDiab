using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Controller;

namespace WizardsCode.MinDiab.UI
{
    public abstract class BaseHUDElement : MonoBehaviour
    {
        internal CharacterRoleController Player;

        private void Awake()
        {
            Player = GameObject.FindObjectOfType<CharacterRoleController>();
        }

        public abstract void UpdateUINormalized(float normalizedValue);
    }
}
