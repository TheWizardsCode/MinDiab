using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Character;
using WizardsCode.MinDiab.Combat;

namespace WizardsCode.MinDiab.Controller
{

    public class AIController : MonoBehaviour
    {
        [SerializeField, Tooltip("If an enemy is within this distance then give chase.")]
        float m_ChaseDistance = 5f;
        [SerializeField, Tooltip("The location that this AI is set to guard. If Vector3.zero it will automatically be set to the start location of the AI.")]
        Vector3 guardPosition;

        Fighter fighter;
        HealthController health;
        Mover mover;
        HealthController player;
        float chaseDistanceSqr;
        bool isAttacking = false;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<HealthController>();
            mover = GetComponent<Mover>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
            chaseDistanceSqr = m_ChaseDistance * m_ChaseDistance;

            if (guardPosition == Vector3.zero)
            {
                guardPosition = transform.position;
            }
        }

        private void Update()
        {
            if (health.IsDead) return;

            if (Vector3.SqrMagnitude(transform.position - player.transform.position) <= chaseDistanceSqr)
            {
                isAttacking = true;
                fighter.Attack(player);
            } else if (isAttacking)
            {
                isAttacking = false;
                mover.MoveTo(guardPosition);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, m_ChaseDistance);
        }
    }
}
