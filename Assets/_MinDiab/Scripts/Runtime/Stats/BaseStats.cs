using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField, Tooltip("The characters gender.")]
        CharacterRoleEnum m_Role = CharacterRoleEnum.Grunt;
        [SerializeField, Tooltip("The progression this character is following in their career.")]
        Progression m_Progression = null;

        internal IStatModifierProvider[] modifierProviders;

        private void Awake()
        {
            modifierProviders = GetComponents<IStatModifierProvider>();
        }

        public float GetStat(Stat stat, int level, float baseValue = 0)
        {
            return (baseValue + m_Progression.GetStat(stat, m_Role, level) + GetStatAdditiveModifier(stat)) * GetStatMultiplier(stat);
        }

        float GetStatAdditiveModifier(Stat stat)
        {
            if (modifierProviders == null) return 0;

            float total = 0;
            for (int i = 0; i < modifierProviders.Length; i++)
            {
                foreach(float modifier in modifierProviders[i].GetStatAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        float GetStatMultiplier(Stat stat)
        {
            if (modifierProviders == null) return 1;

            float total = 1;
            for (int i = 0; i < modifierProviders.Length; i++)
            {
                foreach (float modifier in modifierProviders[i].GetStatMultiplier(stat))
                {
                    total += 1 - modifier;
                }
            }
            return total;
        }
    }
}