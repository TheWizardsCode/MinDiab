using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WizardsCode.MinDiab.UI
{
    public class HealthHUDElement : BaseHUDElement
    {
        TMP_Text valueText;

        private void Awake()
        {
            valueText = GetComponent<TMP_Text>();
        }


        public override void UpdateUINormalized(float normalizedValue)
        {
            valueText.text = $"{string.Format("{0:0}%", normalizedValue * 100)}";
        }
    }
}