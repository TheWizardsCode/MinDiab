using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WizardsCode.MinDiab.Controller;

namespace WizardsCode.MinDiab.UI
{
    public class BaseHUDElement : MonoBehaviour
    {
        internal TMP_Text valueText;
        internal CharacterRoleController Player;

        private void Awake()
        {
            Player = GameObject.FindObjectOfType<CharacterRoleController>();
            valueText = GetComponent<TMP_Text>();
        }

        public virtual void UpdateUI(string value)
        {
            if (valueText)
            {
                valueText.text = value;
            }
        }

        public virtual void UpdateUI(float value)
        {
            if (valueText)
            {
                valueText.text = $"{string.Format("{0:0}", value)}";
            }
        }

        public virtual void UpdateUINormalized(float normalizedValue)
        {
            if (valueText)
            {
                valueText.text = $"{string.Format("{0:0}%", normalizedValue * 100)}";
            }
        }
    }
}
