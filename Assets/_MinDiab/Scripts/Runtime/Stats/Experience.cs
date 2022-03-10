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
        [SerializeField, Tooltip("Feedback object to instantiate when the player character up.")]
        GameObject m_LevelUpFeedback;
        [SerializeField, Tooltip("The HUD element, if any on which to display the current experience and/or level.")]
        ExperienceHUDElement m_ExperienceHUDElement;

        public event Action onExperienceGained;
        public event Action onLevelUp;

        int m_CurrentLevel;
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
                    m_ExperiencePoints = value;
                    if (onExperienceGained != null)
                    {
                        onExperienceGained.Invoke();
                    }
                }
            }
        }

        public int Level
        {
            get
            {
                return m_CurrentLevel;
            }
            set
            {
                if (value != m_CurrentLevel)
                {
                    m_CurrentLevel = value;
                    experienceToLevelUp = stats.GetStat(Stat.ExperienceToLevelUp, m_CurrentLevel);
                    if (m_LevelUpFeedback)
                    {
                        Instantiate(m_LevelUpFeedback, transform);
                    }
                    onLevelUp.Invoke();
                }
            }
        }

        private void Awake()
        {
            stats = GetComponent<BaseStats>();
            ExperiencePoints = 0;
            m_CurrentLevel = 1;
            experienceToLevelUp = stats.GetStat(Stat.ExperienceToLevelUp, m_CurrentLevel);
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
                Level++;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (m_ExperienceHUDElement != null)
            {
                m_ExperienceHUDElement.UpdateUI($"{m_CurrentLevel}\n{ExperiencePoints}");
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
                if (stats.GetStat(Stat.ExperienceToLevelUp, m_CurrentLevel) < ExperiencePoints)
                {
                    m_CurrentLevel++;
                } else
                {
                    foundLevel = true;
                }
            }
            experienceToLevelUp = stats.GetStat(Stat.ExperienceToLevelUp, m_CurrentLevel);

            UpdateUI();
        }
    }
}
