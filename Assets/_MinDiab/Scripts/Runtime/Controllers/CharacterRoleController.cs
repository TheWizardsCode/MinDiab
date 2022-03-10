using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using WizardsCode.MinDiab.Character;
using WizardsCode.MinDiab.Combat;
using WizardsCode.MinDiab.Core;
using WizardsCode.MinDiab.Stats;

namespace WizardsCode.MinDiab.Controller
{
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(MoveController))]
    [RequireComponent(typeof(Scheduler))]
    [RequireComponent(typeof(Animator))]
    public class CharacterRoleController : MonoBehaviour
    {
        internal Animator animator;
        internal Experience experience;
        internal Fighter fighter;
        internal HealthController health;
        internal MoveController mover;
        internal Scheduler scheduler;
        internal BaseStats stats;
        RuntimeAnimatorController m_DefaultAnimationController;

        public bool IsDead => health.IsDead;
        
        Camera mainCamera;
        Camera MainCamera
        {
            get
            {
                if (!mainCamera)
                {
                    mainCamera = Camera.main;
                }
                return mainCamera;
            }
        }

        internal virtual void Awake()
        {
            animator = GetComponent<Animator>();
            m_DefaultAnimationController = animator.runtimeAnimatorController;
            fighter = GetComponent<Fighter>();
            experience = GetComponent<Experience>();
            health = GetComponent<HealthController>();
            mover = GetComponent<MoveController>();
            scheduler = GetComponent<Scheduler>();
            stats = GetComponent<BaseStats>();
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (health.IsDead) return;
            if (HandleCombatInput()) return;
            if (HandleMovementInput()) return;
        }

        internal float GetStat(Stat stat, float baseValue = 0)
        {
            return stats.GetStat(stat, experience.Level, baseValue);
        }

        public void ResetAnimatorController()
        {
            animator.runtimeAnimatorController = m_DefaultAnimationController;
        }

        RaycastHit[] hits = new RaycastHit[5];

        internal void AddExperience(float amount)
        {
            experience.Add(amount);
        }

        /// <summary>
        /// Handle any active combat inputs.
        /// </summary>
        /// <returns>True if there was a combat interaction in this frame.</returns>
        private bool HandleCombatInput()
        {
            if (!Input.GetMouseButton(0)) return false;

            Array.Clear(hits, 0, hits.Length);

            Physics.RaycastNonAlloc(GetScreenPoint(), hits);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider == null) break;

                HealthController target = hits[i].collider.GetComponent<HealthController>();
                if (fighter.CanAttack(target))
                {
                    return fighter.Attack(target);
                }
            }
            return false;
        }

        internal void StopAllActions()
        {
            scheduler.StopCurrentAction();
        }

        /// <summary>
        /// Move the player to the position of the cursor, if possible.
        /// </summary>
        /// <returns>True if the player has clicked to move, is able to move to the cursor, and has started movement. Otherwise return false.</returns>
        private bool HandleMovementInput()
        {
            if (!Input.GetMouseButton(0)) return false;
            
            NavMeshHit navHit;
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetScreenPoint(), out hit);
            if (hasHit)
            {
                if (NavMesh.SamplePosition(hit.point, out navHit, 0.2f, NavMesh.AllAreas))
                {
                    fighter.StopAction();
                    mover.MoveTo(navHit.position, 1);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get a Ray from the camera to the screenpoint.
        /// </summary>
        /// <returns></returns>
        private Ray GetScreenPoint()
        {
            return MainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
