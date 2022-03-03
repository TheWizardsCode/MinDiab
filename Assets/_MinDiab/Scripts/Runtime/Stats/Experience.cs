using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField]
        float m_ExperiencePoints;

        public void Add(float points)
        {
            m_ExperiencePoints += points;
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
