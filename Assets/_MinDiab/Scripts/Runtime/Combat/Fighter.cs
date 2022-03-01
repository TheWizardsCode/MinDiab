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
        [SerializeField, Tooltip("The currently equipable weapon.")]
        Weapon m_EquippableWeapon;

        float timeOfNextAttack;

        Animator animator;
        HealthController health;
        Scheduler scheduler;
        HealthController combatTarget;

        Weapon equippedItemRightHand;

        private MoveController mover;

        bool IsInRange
        {
            get
            {
                if (equippedItemRightHand == null) { return false;  }

                return Vector3.SqrMagnitude(transform.position - combatTarget.transform.position) < equippedItemRightHand.WeaponRangeSqr;
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<HealthController>();
            mover = GetComponent<MoveController>();
            scheduler = GetComponent<Scheduler>();
            EquipPrimaryWeapon();
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
                    timeOfNextAttack = Time.timeSinceLevelLoad + equippedItemRightHand.TimeBetweenAttacks;
                }
            }
        }

        public void EquipPrimaryWeapon() {
            if (m_EquippableWeapon == null) return;

            m_EquippableWeapon.Equip(m_RightHandMount, animator);
            equippedItemRightHand = m_EquippableWeapon;
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
                combatTarget.TakeDamage(equippedItemRightHand.Damage);
                if (combatTarget.IsDead)
                {
                    StopAction();
                }
            }
        }
    }
}