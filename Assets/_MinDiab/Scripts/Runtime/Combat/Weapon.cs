using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using WizardsCode.MinDiab.Character;

namespace WizardsCode.MinDiab.Combat
{
    public class Weapon : MonoBehaviour
    {
        public enum Handedness { Dominant, NonDominant, BothDominantLead, BothNonDominantLead }

        [SerializeField, Tooltip("The range we need to be within in order to attack.")]
        float m_Range = 2;
        [SerializeField, Tooltip("The time between attacks in seconds.")]
        internal float TimeBetweenAttacks = 1;
        [SerializeField, Tooltip("The amount of damage to do on each hit.")]
        [FormerlySerializedAs("Damage")]
        internal int BaseDamage = 10;
        [SerializeField, Tooltip("The animation controller override to use when the weapon is equipped.")]
        internal AnimatorOverrideController AnimationController;
        [SerializeField, Tooltip("Indicate whether this is a Dominant, Non-Dominant, or both hands. In the case of both hands the dominant and non-dominant versions will indicate which hand the model will be placed in.")]
        internal Handedness Hand = Handedness.Dominant;
        [SerializeField, Tooltip("The projectile this weapon uses. If null then it is assumed no projectile is used.")]
        Projectile projectile = null;
        [SerializeField, Tooltip("The name of the transform which marks position from which the project will fire. Leave as null to make the launch point the same as the weapon mount point - useful for spells and similar that don't have a model and come from the hand.")]
        Transform m_ProjectileLaunchPoint;

        [Header("Events")]
        [SerializeField, Tooltip("Events to fire when this projectile is launched.")]
        UnityEvent m_OnStartAttack;
        [SerializeField, Tooltip("Events to fire when this projectile hits something..")]
        UnityEvent m_OnHit;


        float? rangeSqr = null;
        internal float WeaponRangeSqr { 
            get
            {
                if (rangeSqr == null)
                {
                    rangeSqr = m_Range * m_Range;
                }
                return (float)rangeSqr;
            }
        }

        public bool HasProjectile
        {
            get { return projectile != null; }
        }

        /// <summary>
        /// Launch a projectile at a target.
        /// </summary>
        /// <param name="target">The target we are trying to hit.</param>
        /// <param name="fighter">The fighter launching the attack.</param>
        /// <param name="totalDamage">The total damage after all additives and muiltipliers have been applied.</param>
        public void LaunchProjectileAt(HealthController target, Fighter fighter, float totalDamage)
        {
            m_OnStartAttack.Invoke();
            Vector3 launchPoint = fighter.transform.position;
            if (m_ProjectileLaunchPoint)
            {
                launchPoint += m_ProjectileLaunchPoint.localPosition;
            } else
            {
                launchPoint = transform.position;
            }
            Projectile projectileInstance = Instantiate(projectile, launchPoint, projectile.transform.rotation);
            projectileInstance.Launch(target, totalDamage, fighter);
        }

        /// <summary>
        /// Get the additive damage this particular weapon does. This will be in addition to the base damage done and will
        /// be effected by buffs and nerfs on the weapon.
        /// </summary>
        public float DamageAdditive
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Get the multiplier for the damage this particular weapon does. This will be applied after the additive damage has been added to the base damage.
        /// </summary>
        public float DamageMultiplier
        {
            get
            {
                return 1;
            }
        }
    }
}