using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Character;
using WizardsCode.MinDiab.Configuration;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.Combat
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Scheduler))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(HealthController))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField, Tooltip("The range we need to be within in order to attack.")]
        float m_WeaponRange = 2;
        [SerializeField, Tooltip("The time between attacks in seconds.")]
        float m_TimeBetweenAttacks = 1;
        [SerializeField, Tooltip("The amount of damage to do on each hit.")]
        int m_Damage = 10;

        float weaponRangeSqr;
        float timeOfNextAttack;

        Animator animator;
        Scheduler scheduler;
        HealthController combatTarget;

        private Mover mover;

        bool IsInRange
        {
            get
            {
                return Vector3.SqrMagnitude(transform.position - combatTarget.transform.position) < weaponRangeSqr;
            }
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            scheduler = GetComponent<Scheduler>();
            weaponRangeSqr = m_WeaponRange * m_WeaponRange;
        }

        private void Update()
        {
            if (!combatTarget) return;

            if (!IsInRange)
            {
                mover.MoveTo(combatTarget.transform.position);
            } else
            {
                if (Time.timeSinceLevelLoad > timeOfNextAttack)
                {
                    mover.StopAction();
                    animator.SetTrigger(AnimationParameters.DefaultAttackTriggerID);
                    timeOfNextAttack = Time.timeSinceLevelLoad + m_TimeBetweenAttacks;
                }
            }
        }

        /// <summary>
        /// Try to attack a target. If this Fighter cannot attack the current target then return false.
        /// Otherwise commence the attack.
        /// </summary>
        /// <param name="target">The target to try to attack.</param>
        /// <returns>True if this Fighter can attack the target (possibly some preparation, such as moving). False if the Fighter cannot attack.</returns>
        public bool Attack(HealthController target)
        {
            if (CanAttack(target)) { 
                transform.LookAt(target.transform);
                scheduler.StartAction(this);
                combatTarget = target;
                return true;
            } else {
                return false;
            }
        }

        public bool CanAttack(HealthController target)
        {
            if (!target) return false;

            return !target.IsDead;
        }

        /// <summary>
        /// Cancel the fighters interest in a combat target.
        /// </summary>
        public void StopAction()
        {
            combatTarget = null;
            animator.SetTrigger(AnimationParameters.StopActionTriggerID);
        }

        /// <summary>
        /// Handle the animation Hit event.
        /// </summary>
        void Hit()
        {
            if (combatTarget)
            {
                combatTarget.TakeDamage(m_Damage);
                if (combatTarget.IsDead)
                {
                    StopAction();
                }
            }
        }
    }
}