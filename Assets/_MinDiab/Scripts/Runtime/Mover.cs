using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.Character
{
    [RequireComponent(typeof(Scheduler))]
    public class Mover : MonoBehaviour, IAction
    {
        const string ANIMATOR_FORWARD_SPEED = "forwardSpeed";

        private NavMeshAgent agent;
        private Animator animator;
        Scheduler scheduler;
        private int forwardSpeedParameter;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            scheduler = GetComponent<Scheduler>();
            forwardSpeedParameter = Animator.StringToHash(ANIMATOR_FORWARD_SPEED);
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void MoveTo(Vector3 pos)
        {
            scheduler.StartAction(this);
            agent.SetDestination(pos);
            agent.isStopped = false;
        }

        public void StopAction()
        {
            agent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
            float speed = localVelocity.z;
            animator.SetFloat(forwardSpeedParameter, speed);
        }
    }
}
