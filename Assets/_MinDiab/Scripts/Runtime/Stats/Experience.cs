using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Stats
{
    public class Experience : MonoBehaviour
    {
        [SerializeField]
        float m_ExperiencePoints;

        public void Add(float points)
        {
            m_ExperiencePoints += points;
        }
    }
}
