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
        ExperienceHUDElement m_HUDElement;

        int currentLevel;
        private BaseStats stats;
        float experiencePoints;
        float experienceToLevelUp; 
        
        public int Level
        {
            get
            {
                return currentLevel;
            }
            set
            {
                currentLevel = value;
            }
        }

        private void Awake()
        {
            stats = GetComponent<BaseStats>();
            experiencePoints = 0;
            currentLevel = 1;
            experienceToLevelUp = stats.GetStat(Stat.ExperienceToLevelUp, currentLevel);
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (m_HUDElement != null)
            {
                m_HUDElement.UpdateUI(experiencePoints);
            }
        }

        public void Add(float points)
        {
            experiencePoints += points;
            if (experiencePoints > experienceToLevelUp)
            {
                // TODO: Level Up Feedback
            }
            UpdateUI();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;

            bool foundLevel = false;
            while(!foundLevel)
            {
                if (stats.GetStat(Stat.ExperienceToLevelUp, currentLevel + 1) < experiencePoints)
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
