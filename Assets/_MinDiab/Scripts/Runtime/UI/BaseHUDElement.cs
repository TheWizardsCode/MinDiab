using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Controller;

namespace WizardsCode.MinDiab.UI
{
    public abstract class BaseHUDElement : MonoBehaviour
    {
        internal PlayerController Player;

        private void Awake()
        {
            Player = GameObject.FindObjectOfType<PlayerController>();
        }

        public abstract void UpdateUINormalized(float normalizedValue);
    }
}
