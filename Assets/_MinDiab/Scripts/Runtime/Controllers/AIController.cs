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

        Fighter fighter;
        Mover mover;
        HealthController player;
        float chaseDistanceSqr;
        bool isAttacking = false;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
            chaseDistanceSqr = m_ChaseDistance * m_ChaseDistance;
        }

        private void Update()
        {
            if (Vector3.SqrMagnitude(transform.position - player.transform.position) <= chaseDistanceSqr)
            {
                fighter.Attack(player);
                isAttacking = true;
            } else if (isAttacking)
            {
                isAttacking = false;
                fighter.StopAction();
            }
        }
    }
}
