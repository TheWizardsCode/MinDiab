using System;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Character;
using WizardsCode.MinDiab.Configuration;
using WizardsCode.MinDiab.Controller;
using WizardsCode.MinDiab.Core;
using WizardsCode.MinDiab.Stats;
using static WizardsCode.MinDiab.Combat.Weapon;

namespace WizardsCode.MinDiab.Combat
{
    [RequireComponent(typeof(CharacterRoleController))]
    public class Fighter : MonoBehaviour, IAction, IStatModifierProvider
    {
        [SerializeField, Tooltip("The currently equipable weapon.")]
        Weapon m_DefaultWeapon;
        [SerializeField, Tooltip("The position to equip items in the dominant hand.")]
        Transform m_DominantMount;
        [SerializeField, Tooltip("The position to equip items in the non-dominant hand.")]
        Transform m_NonDominantMount;

        float timeOfNextAttack;

        CharacterRoleController controller;
        HealthController combatTarget;

        Weapon equippedWeaponDominantHand; 
        Weapon equippedWeaponNonDominantHand;

        private void Awake()
        {
            controller = GetComponent<CharacterRoleController>();
        }

        private void Start()
        {
            EquipWeapon(m_DefaultWeapon);
        }

        private void Update()
        {
            if (!CanAttack(combatTarget) || controller.IsDead)
            {
                return;
            }

            if (!IsInRange(combatTarget))
            {
                controller.mover.MoveTo(combatTarget.transform.position, 1);
            }
            else
            {
                if (Time.timeSinceLevelLoad > timeOfNextAttack)
                {
                    controller.mover.StopAction();
                    controller.animator.SetTrigger(AnimationParameters.DefaultAttackTriggerID);
                    timeOfNextAttack = Time.timeSinceLevelLoad + SelectedWeapon.TimeBetweenAttacks;
                }
            }
        }

        public bool IsInRange(HealthController target)
        {
            if (SelectedWeapon == null) { return false; }

            if (Vector3.SqrMagnitude(transform.position - target.transform.position) < SelectedWeapon.WeaponRangeSqr)
            {
                return true;
            }
  
            return false;
        }

        /// <summary>
        /// Select a weapon to attack the target from the ones currently equipped.
        /// </summary>
        /// <returns>The weapon to use, or null if no suitable weapon is available.</returns>
        private Weapon SelectedWeapon
        {
            get
            {
                if (equippedWeaponDominantHand != null)
                {
                    return equippedWeaponDominantHand;
                }
                if (equippedWeaponNonDominantHand != null)
                {
                    return equippedWeaponNonDominantHand;
                }
                return null;
            }
        }

        public void EquipWeapon(Weapon weapon) {
            if (weapon == null) return;

            DiscardItemInHand(weapon.Hand);

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
                controller.animator.runtimeAnimatorController = weapon.AnimationController;
            } else
            {
                controller.ResetAnimatorController();
            }
        }

        public void DiscardItemInHand(Handedness hand)
        {
            switch (hand)
            {
                case Handedness.Dominant:
                    if (equippedWeaponDominantHand)
                    {
                        Destroy(equippedWeaponDominantHand.gameObject);
                    }
                    break;
                case Handedness.NonDominant:
                    if (equippedWeaponDominantHand)
                    {
                        Destroy(equippedWeaponDominantHand.gameObject);
                    }
                    break;
                case Handedness.BothDominantLead:
                    if (equippedWeaponDominantHand)
                    {
                        Destroy(equippedWeaponDominantHand.gameObject);
                    }
                    if (equippedWeaponDominantHand)
                    {
                        Destroy(equippedWeaponDominantHand.gameObject);
                    }
                    break;
                case Handedness.BothNonDominantLead:
                    if (equippedWeaponDominantHand)
                    {
                        Destroy(equippedWeaponDominantHand.gameObject);
                    }
                    if (equippedWeaponDominantHand)
                    {
                        Destroy(equippedWeaponDominantHand.gameObject);
                    }
                    break;
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
                controller.scheduler.StartAction(this);
                combatTarget = target;
                return true;
            } else {
                return false;
            }
        }

        public bool CanAttack(HealthController target)
        {
            if (!target) return false;

            if (!SelectedWeapon) return false;

            if (target == controller.health)
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
            controller.animator.SetTrigger(AnimationParameters.StopActionTriggerID);
            controller.mover.StopAction();
        }

        /// <summary>
        /// Handle the animation Hit event.
        /// </summary>
        void Hit()
        {
            float damage = controller.GetStat(Stat.Damage, SelectedWeapon.BaseDamage);
            if (combatTarget)
            {
                if (SelectedWeapon.HasProjectile)
                {
                    SelectedWeapon.LaunchProjectileAt(combatTarget, this, damage);
                }
                else
                {
                    combatTarget.TakeDamage( damage, this);
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

        public IEnumerable<float> GetStatAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return SelectedWeapon.DamageAdditive;
            }
        }

        public IEnumerable<float> GetStatMultiplier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return SelectedWeapon.DamageMultiplier;
            }
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