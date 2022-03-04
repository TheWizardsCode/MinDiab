using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Core;
using WizardsCode.MinDiab.UI;

namespace WizardsCode.MinDiab.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField]
        ExperienceHUDElement m_HUDElement;

        float m_ExperiencePoints;

        public void Add(float points)
        {
            m_ExperiencePoints += points;
            m_HUDElement.UpdateUI(m_ExperiencePoints);
        }

        public object CaptureState()
        {
            return m_ExperiencePoints;
        }

        public void RestoreState(object state)
        {
            m_ExperiencePoints = (float)state;
        }
    }
}
