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

        public float Health(CharacterRole role, int level)
        {
            for (int i = 0; i < m_CharacterRoles.Length; i++) {
                if (m_CharacterRoles[i].Role == role) {
                    return m_CharacterRoles[i].Health[level - 1];
                }
            }
            return 0;
        }

        [Serializable]
        class ProgressionCharacterRole
        {
            [SerializeField, Tooltip("The Character class this progression represents is for")]
            internal CharacterRole Role = CharacterRole.Grunt;
            [SerializeField, Tooltip("The base health for each level in the role.")]
            internal float[] Health;
        }
    }
}