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

        public float GetStat(Stat stat, int level)
        {
            return m_Progression.GetStat(stat, m_Role, level);
        }
    }
}