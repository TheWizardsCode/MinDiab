﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using WizardsCode.MinDiab.Character;
using WizardsCode.MinDiab.Combat;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.Controller
{
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(MoveController))]
    [RequireComponent(typeof(Scheduler))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        internal Animator animator;
        internal Fighter fighter;
        internal HealthController health;
        internal MoveController mover;
        internal Scheduler scheduler;
        Camera mainCamera;
        RuntimeAnimatorController m_DefaultAnimationController;

        public bool IsDead => health.IsDead;

        internal virtual void Start()
        {
            animator = GetComponent<Animator>();
            m_DefaultAnimationController = animator.runtimeAnimatorController;
            fighter = GetComponent<Fighter>();
            health = GetComponent<HealthController>();
            mover = GetComponent<MoveController>();
            scheduler = GetComponent<Scheduler>();
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (health.IsDead) return;
            if (HandleCombatInput()) return;
            if (HandleMovementInput()) return;
        }

        public void ResetAnimatorController()
        {
            animator.runtimeAnimatorController = m_DefaultAnimationController;
        }

        RaycastHit[] hits = new RaycastHit[5];
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
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
