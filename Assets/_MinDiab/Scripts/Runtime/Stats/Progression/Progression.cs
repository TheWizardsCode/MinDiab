using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Stats
{
    [CreateAssetMenu(fileName ="New Progression", menuName="Wizards Code/Stats/New Progression")]
    public class Progression : ScriptableObject {
        [SerializeField, Tooltip("The progression data for character classes.")]
        ProgressionCharacterRole[] m_CharacterRoles = null;

        Dictionary<CharacterRoleEnum, Dictionary<Stat, float[]>> statDictionary = new Dictionary<CharacterRoleEnum, Dictionary<Stat, float[]>>();

        public float GetStat(Stat stat, CharacterRoleEnum role, int level)
        {
            BuildStatDictionary();

            Dictionary<Stat, float[]> statLookup;
            if (statDictionary.TryGetValue(role, out statLookup)) {
                float[] values;
                if (statLookup.TryGetValue(stat, out values)) {
                    return values[level - 1];
                }
            }

            return 0;
        }

        public float GetExperienceRequiredToLevelUp(CharacterRoleEnum role, int currentLevel)
        {
            BuildStatDictionary();
            Dictionary<Stat, float[]> statLookup;
            if (statDictionary.TryGetValue(role, out statLookup))
            {
                float[] values;
                if (statLookup.TryGetValue(Stat.ExperienceToLevelUp, out values))
                {
                    return values[currentLevel - 1];
                }
            }
            return 0;
        }

        private void BuildStatDictionary()
        {
            if (statDictionary.Count > 0) return;

            Dictionary<Stat, float[]> stat = new Dictionary<Stat, float[]>();

            for (int i = 0; i < m_CharacterRoles.Length; i++)
            {
                for (int y = 0; y < m_CharacterRoles[i].stats.Length; y++)
                {
                    stat.Add(m_CharacterRoles[i].stats[y].stat, m_CharacterRoles[i].stats[y].values);
                }
                statDictionary.Add(m_CharacterRoles[i].Role, stat);
                stat = new Dictionary<Stat, float[]>();
            }
        }

        [Serializable]
        class ProgressionCharacterRole
        {
            [SerializeField, Tooltip("The Character class this progression represents is for")]
            internal CharacterRoleEnum Role = CharacterRoleEnum.Grunt;
            [SerializeField, Tooltip("The stats for this character role.")]
            internal ProgressionStat[] stats;
            //[SerializeField, Tooltip("The base health for each level in the role.")]
            //internal float[] Health;
        }

        [Serializable]
        class ProgressionStat
        {
            [SerializeField, Tooltip("The stat represented here.")]
            internal Stat stat;
            [SerializeField, Tooltip("The base value at different levels.")]
            internal float[] values;
        }
    }
}