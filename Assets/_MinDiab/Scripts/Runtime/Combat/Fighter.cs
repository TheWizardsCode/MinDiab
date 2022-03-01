using System;
using UnityEngine;
using WizardsCode.MinDiab.Character;
using WizardsCode.MinDiab.Configuration;
using WizardsCode.MinDiab.Core;
using static WizardsCode.MinDiab.Combat.Weapon;

namespace WizardsCode.MinDiab.Combat
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Scheduler))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(HealthController))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField, Tooltip("The currently equipable weapon.")]
        Weapon m_DefaultWeapon;
        [SerializeField, Tooltip("The position to equip items in the dominant hand.")]
        Transform m_DominantMount;
        [SerializeField, Tooltip("The position to equip items in the non-dominant hand.")]
        Transform m_NonDominantMount;

        float timeOfNextAttack;

        Animator animator;
        HealthController health;
        Scheduler scheduler;
        HealthController combatTarget;

        Weapon equippedWeaponDominantHand; 
        Weapon equippedWeaponNonDominantHand;

        private MoveController mover;

        bool IsInRange
        {
            get
            {
                if (equippedWeaponDominantHand == null) { return false;  }

                if (equippedWeaponDominantHand && 
                    Vector3.SqrMagnitude(transform.position - combatTarget.transform.position) < equippedWeaponDominantHand.WeaponRangeSqr) 
                { 
                    return true; 
                }

                if (equippedWeaponNonDominantHand &&
                    Vector3.SqrMagnitude(transform.position - combatTarget.transform.position) < equippedWeaponNonDominantHand.WeaponRangeSqr) 
                { 
                    return true; 
                }

                return false;
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<HealthController>();
            mover = GetComponent<MoveController>();
            scheduler = GetComponent<Scheduler>();
            EquipWeapon(m_DefaultWeapon);
        }

        private void Update()
        {
            if (!CanAttack(combatTarget) || health.IsDead) { 
                return; 
            }

            if (equippedWeaponDominantHand == null)
            {
                EquipWeapon(m_DefaultWeapon);
            }

            if (!IsInRange)
            {
                mover.MoveTo(combatTarget.transform.position, 1);
            } else
            {
                if (Time.timeSinceLevelLoad > timeOfNextAttack)
                {
                    mover.StopAction();
                    animator.SetTrigger(AnimationParameters.DefaultAttackTriggerID);
                    timeOfNextAttack = Time.timeSinceLevelLoad + equippedWeaponDominantHand.TimeBetweenAttacks;
                }
            }
        }

        public void EquipWeapon(Weapon weapon) {
            if (weapon == null) return;

            switch (weapon.Hand)
            {
                case Handedness.Dominant:
                    equippedWeaponDominantHand = Instantiate(weapon.gameObject, m_DominantMount).GetComponent<Weapon>();
                    equippedWeaponDominantHand.transform.SetParent(m_DominantMount);
                    break;
                case Handedness.NonDominant:
                    equippedWeaponNonDominantHand = Instantiate(weapon.gameObject, m_NonDominantMount).GetComponent<Weapon>();
                    equippedWeaponNonDominantHand.transform.SetParent(m_NonDominantMount);
                    break;
                case Handedness.BothDominantLead:
                    equippedWeaponDominantHand = Instantiate(weapon.gameObject, m_DominantMount).GetComponent<Weapon>();
                    equippedWeaponDominantHand.transform.SetParent(m_DominantMount);
                    equippedWeaponNonDominantHand = equippedWeaponDominantHand;
                    break;
                case Handedness.BothNonDominantLead:
                    equippedWeaponNonDominantHand = Instantiate(weapon.gameObject, m_NonDominantMount).GetComponent<Weapon>();
                    equippedWeaponNonDominantHand.transform.SetParent(m_NonDominantMount);
                    equippedWeaponDominantHand = equippedWeaponNonDominantHand;
                    break;
            }

            if (weapon.AnimationController != null)
            {
                animator.runtimeAnimatorController = weapon.AnimationController;
            }

            equippedWeaponDominantHand = weapon;
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
            if (target == health)
            {
                return false;
            }

            return !target.IsDead;
        }

        /// <summary>
        /// Cancel the fighters interest in a combat target.
        /// </summary>
        public void StopAction()
        {
            animator.SetTrigger(AnimationParameters.StopActionTriggerID);
            mover.StopAction();
        }

        /// <summary>
        /// Handle the animation Hit event.
        /// </summary>
        void Hit()
        {
            if (combatTarget)
            {
                if (equippedWeaponDominantHand.HasProjectile)
                {
                    equippedWeaponDominantHand.LaunchProjectileAt(combatTarget, this);
                }
                else
                {
                    combatTarget.TakeDamage(equippedWeaponDominantHand.Damage);
                }

                if (combatTarget.IsDead)
                {
                    StopAction();
                }
            }
        }

        void Shoot()
        {
            Hit();
        }

        /*
        public object CaptureState()
        {
            SaveData data = new SaveData();
            data.defaultWeapon = m_DefaultWeapon;
            data.equippedRightHand = equippedWeaponRightHand;
            return data;
        }

        public void RestoreState(object state)
        {
            SaveData data = (SaveData)state;
            m_DefaultWeapon = data.defaultWeapon;
            equippedWeaponRightHand = data.equippedRightHand;
        }

        [Serializable]
        struct SaveData
        {
            public Weapon defaultWeapon;
            public Weapon equippedRightHand;
        }
        */
    }
}