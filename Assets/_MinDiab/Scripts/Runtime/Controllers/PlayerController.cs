using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WizardsCode.MinDiab.Character;
using WizardsCode.MinDiab.Combat;

namespace WizardsCode.MinDiab.Controller
{
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(Mover))]
    public class PlayerController : MonoBehaviour
    {
        Fighter fighter;
        Mover mover;
        Camera mainCamera;

        private void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (HandleCombatInput()) return;
            if (HandleMovementInput()) return;
        }

        RaycastHit[] hits = new RaycastHit[5];
        private bool HandleCombatInput()
        {
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

        /// <summary>
        /// Move the player to the position of the cursor, if possible.
        /// </summary>
        /// <returns>True if the player is able to move to the cursor and has started movement. Otherwise return false.</returns>
        private bool HandleMovementInput()
        {
            NavMeshHit navHit;
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetScreenPoint(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    if (NavMesh.SamplePosition(hit.point, out navHit, 0.2f, NavMesh.AllAreas))
                    {
                        fighter.StopAction();
                        mover.MoveTo(navHit.position);
                        return true;
                    }
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
