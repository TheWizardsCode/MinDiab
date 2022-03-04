using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Controller;
using WizardsCode.MinDiab.Core;
using WizardsCode.MinDiab.UI;

namespace WizardsCode.MinDiab.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField]
        ExperienceHUDElement m_ExperienceHUDElement;

        public event Action onExperienceGained;

        int currentLevel;
        private BaseStats stats;
        float experienceToLevelUp;

        float m_ExperiencePoints;
        float ExperiencePoints
        {
            get
            {
                return m_ExperiencePoints;
            }
            set
            {
                if (value != m_ExperiencePoints)
                {
                    m_ExperiencePoints += value;
                    onExperienceGained.Invoke();
                }
            }
        }

        public int Level
        {
            get
            {
                return currentLevel;
            }
            set
            {
                if (value != currentLevel)
                {
                    currentLevel = value;
                    experienceToLevelUp = stats.GetStat(Stat.ExperienceToLevelUp, currentLevel);
                    // TODO: Level Up Feedback
                }
            }
        }

        private void Awake()
        {
            stats = GetComponent<BaseStats>();
            ExperiencePoints = 0;
            currentLevel = 1;
            experienceToLevelUp = stats.GetStat(Stat.ExperienceToLevelUp, currentLevel);
        }

        private void OnEnable()
        {
            onExperienceGained += OnExperienceGained;
        }

        private void OnDisable()
        {
            onExperienceGained -= OnExperienceGained;
        }

        private void OnExperienceGained()
        {
            if (ExperiencePoints >= experienceToLevelUp)
            {
                currentLevel++;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (m_ExperienceHUDElement != null)
            {
                m_ExperienceHUDElement.UpdateUI($"{currentLevel}\n{ExperiencePoints}");
            }
        }

        public void Add(float points)
        {
            ExperiencePoints += points;
        }

        public object CaptureState()
        {
            return ExperiencePoints;
        }

        public void RestoreState(object state)
        {
            m_ExperiencePoints = (float)state;

            bool foundLevel = false;
            while(!foundLevel)
            {
                if (stats.GetStat(Stat.ExperienceToLevelUp, currentLevel) < ExperiencePoints)
                {
                    currentLevel++;
                } else
                {
                    foundLevel = true;
                }
            }
            experienceToLevelUp = stats.GetStat(Stat.ExperienceToLevelUp, currentLevel);

            UpdateUI();
        }
    }
}
