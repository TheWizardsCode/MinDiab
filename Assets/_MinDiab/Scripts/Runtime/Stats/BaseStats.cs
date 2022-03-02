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

        public float Health
        {
            get
            {
                return m_Progression.Health(m_Role, m_Level);
            }
        }
    }
}