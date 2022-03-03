using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField, Tooltip("The level of this character.")]
        [Range(0,1000)]
        int m_Level = 1;
        [SerializeField, Tooltip("The characters gender.")]
        CharacterRole m_Role = CharacterRole.Grunt;
        [SerializeField, Tooltip("The progression this character is following in their career.")]
        Progression m_Progression = null;

        public int Level
        {
            get
            {
                return m_Level;
            }
            set
            {
                m_Level = value;
            }
        }

        public float GetStat(Stat stat)
        {
            return m_Progression.GetStat(stat, m_Role, m_Level);
        }
    }
}