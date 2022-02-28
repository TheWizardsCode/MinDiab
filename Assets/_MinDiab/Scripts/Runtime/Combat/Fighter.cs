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
        [SerializeField, Tooltip("The position to equip items in the right hand.")]
        Transform m_RightHandMount;

        [Header("Weapon")]
        [SerializeField, Tooltip("The weapon to equip.")]
        GameObject m_WeaponPrefab = null;
        [SerializeField, Tooltip("The range we need to be within in order to attack.")]
        float m_WeaponRange = 2;
        [SerializeField, Tooltip("The time between attacks in seconds.")]
        float m_TimeBetweenAttacks = 1;
        [SerializeField, Tooltip("The amount of damage to do on each hit.")]
        int m_Damage = 10;
        [SerializeField, Tooltip("The animation controller override to use when the weapon is equipped.")]
        AnimatorOverrideController weaponAnimationController;


        float weaponRangeSqr;
        float timeOfNextAttack;

        Animator animator;
        HealthController health;
        Scheduler scheduler;
        HealthController combatTarget;

        GameObject equippedItemRightHand;

        private MoveController mover;

        bool IsInRange
        {
            get
            {
                return Vector3.SqrMagnitude(transform.position - combatTarget.transform.position) < weaponRangeSqr;
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<HealthController>();
            mover = GetComponent<MoveController>();
            scheduler = GetComponent<Scheduler>();
            weaponRangeSqr = m_WeaponRange * m_WeaponRange;
        }

        private void Update()
        {
            if (!CanAttack(combatTarget) || health.IsDead) { 
                return; 
            }

            if (equippedItemRightHand == null)
            {
                EquipPrimaryWeapon();
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
                    timeOfNextAttack = Time.timeSinceLevelLoad + m_TimeBetweenAttacks;
                }
            }
        }

        public void EquipPrimaryWeapon() {
            if (m_WeaponPrefab)
            {
                equippedItemRightHand = Instantiate(m_WeaponPrefab, m_RightHandMount);
                animator.runtimeAnimatorController = weaponAnimationController;
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
                combatTarget.TakeDamage(m_Damage);
                if (combatTarget.IsDead)
                {
                    StopAction();
                }
            }
        }
    }
}