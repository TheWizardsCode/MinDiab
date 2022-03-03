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

        public float GetStat(Stat stat, CharacterRole role, int level)
        {
            for (int i = 0; i < m_CharacterRoles.Length; i++) {
                if (m_CharacterRoles[i].Role == role) {
                    for (int y = 0; y < m_CharacterRoles[i].stats.Length; y++)
                    {
                        if (m_CharacterRoles[i].stats[y].stat == stat)
                        {
                            return m_CharacterRoles[i].stats[y].values[level - 1];
                        }
                    }
                }
            }
            return 0;
        }

        [Serializable]
        class ProgressionCharacterRole
        {
            [SerializeField, Tooltip("The Character class this progression represents is for")]
            internal CharacterRole Role = CharacterRole.Grunt;
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