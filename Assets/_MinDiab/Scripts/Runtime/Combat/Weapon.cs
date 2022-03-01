using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Wizards Code/MinDiab/New Weapon")]
    public class Weapon : ScriptableObject
    {
        [SerializeField, Tooltip("The weapon to equip.")]
        GameObject m_WeaponPrefab = null;
        [SerializeField, Tooltip("The range we need to be within in order to attack.")]
        float m_WeaponRange = 2;
        [SerializeField, Tooltip("The time between attacks in seconds.")]
        internal float TimeBetweenAttacks = 1;
        [SerializeField, Tooltip("The amount of damage to do on each hit.")]
        internal int Damage = 10;
        [SerializeField, Tooltip("The animation controller override to use when the weapon is equipped.")]
        AnimatorOverrideController m_WeaponAnimationController;

        [HideInInspector]
        internal float WeaponRangeSqr { get; private set; }

        private void Awake()
        {
            WeaponRangeSqr = m_WeaponRange * m_WeaponRange;
        }

        public void Equip(Transform mountPoint, Animator animator)
        {
            if (m_WeaponPrefab != null)
            {
                if (mountPoint != null)
                {
                    Instantiate(m_WeaponPrefab, mountPoint);
                } else
                {
                    Instantiate(m_WeaponPrefab);
                }
            }

            if (m_WeaponAnimationController != null)
            {
                animator.runtimeAnimatorController = m_WeaponAnimationController;
            }
        }
    }
}