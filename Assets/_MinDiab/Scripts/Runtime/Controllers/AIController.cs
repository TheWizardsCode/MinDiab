using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Character;
using WizardsCode.MinDiab.Combat;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.Controller
{

    public class AIController : CharacterRoleController
    {
        [SerializeField, Tooltip("If an enemy is within this distance then give chase.")]
        float m_ChaseDistance = 5f;
        [SerializeField, Tooltip("The location that this AI is set to guard. If Vector3.zero it will automatically be set to the first wapoint in the patrol path, if one is provided, otherwise it will be set to the start location of the AI.")]
        Vector3 m_GuardPosition;
        [SerializeField, Tooltip("The time this AI will remain suspicious wants they are made aware of an enemy.")]
        float m_SuspicionDuration = 4;
        [SerializeField, Tooltip("The patrol path the AI shuold follow, if any. This will override any assigned guard position.")]
        PatrolController m_PatrolPath;
        [SerializeField, Tooltip("The starting waypoint for this character if they have a patrol path assigned.")]
        int m_CurrentWaypointIndex = 0;


        HealthController m_Player;
        float m_ChaseDistanceSqr;
        bool m_IsAttacking = false;

        float timeToEndDwell = Mathf.NegativeInfinity;

        private bool ShouldAttack
        {
            get
            {
                bool attack = IsAggravated;
                attack |= Vector3.SqrMagnitude(transform.position - m_Player.transform.position) <= m_ChaseDistanceSqr;

                return attack;
            }
        }

        internal override void Awake()
        {
            base.Awake();
            m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
        }

        private void Start()
        {
            m_ChaseDistanceSqr = m_ChaseDistance * m_ChaseDistance;

            if (m_GuardPosition == Vector3.zero)
            {
                if (m_PatrolPath)
                {
                    m_GuardPosition = m_PatrolPath.GetWaypointPosition(m_CurrentWaypointIndex);
                }
                else
                {
                    m_GuardPosition = transform.position;
                }
            }

            mover.Warp(m_GuardPosition);
        }

        void Update()
        {
            if (health.IsDead) return;

            if (ShouldAttack)
            {
                AttackBehaviour();
            }
            else if (m_IsAttacking && timeToEndDwell == Mathf.NegativeInfinity)
            {
                timeToEndDwell = Time.timeSinceLevelLoad + m_SuspicionDuration;
                scheduler.StopCurrentAction();
            }
            else if (m_IsAttacking && Time.timeSinceLevelLoad < timeToEndDwell)
            {
                // not in range but we are suspicious, do nothing
            }
            else if (m_IsAttacking)
            {
                // we are attacking and not suspuscious anymore so go back to the 
                m_IsAttacking = false;
                timeToEndDwell = Mathf.NegativeInfinity;
                mover.MoveTo(m_GuardPosition, 1);
            }
            else if (m_PatrolPath)
            {
                PatrolBehaviour();
            }
            else
            {
                GuardBehaviur();
            }
        }

        private void PatrolBehaviour()
        {
            if (mover.AtDestination)
            {
                if (timeToEndDwell == Mathf.NegativeInfinity)
                {
                    timeToEndDwell = Time.timeSinceLevelLoad + m_PatrolPath.DwellTime;
                }
                else if (Time.timeSinceLevelLoad > timeToEndDwell) 
                {
                    m_CurrentWaypointIndex = m_PatrolPath.GetNextWaypointIndex(m_CurrentWaypointIndex);
                    mover.MoveTo(m_PatrolPath.GetWaypointPosition(m_CurrentWaypointIndex), m_PatrolPath.SpeedMultiplier);
                    timeToEndDwell = Mathf.NegativeInfinity;
                }
            }
        }

        private void GuardBehaviur()
        {
            // Just stand there for now.
        }

        /// <summary>
        /// AI is within the chase distance so attack target (which may include a move)
        /// </summary>
        private void AttackBehaviour()
        {
            m_IsAttacking = true;
            fighter.Attack(m_Player);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, m_ChaseDistance);
        }
    }
}
