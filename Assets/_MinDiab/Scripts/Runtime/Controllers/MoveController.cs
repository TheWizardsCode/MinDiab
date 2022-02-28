using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WizardsCode.MinDiab.Configuration;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.Character
{
    [RequireComponent(typeof(Scheduler))]
    public class MoveController : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField, Tooltip("The maximum speed of mocement under normal circumstances, i.e. with no buffs or nerfs.")]
        float m_MaxSpeed = 6;

        private NavMeshAgent agent;
        private Animator animator;
        Scheduler scheduler;

        public bool AtDestination {
            get
            {
                return !agent.hasPath || agent.remainingDistance < agent.stoppingDistance;
            }
        }

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            scheduler = GetComponent<Scheduler>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void MoveTo(Vector3 pos, float speedMultiplier)
        {
            agent.speed = m_MaxSpeed * speedMultiplier;

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
            animator.SetFloat(AnimationParameters.ForwardSpeed, speed);
        }

        internal void Warp(Vector3 position)
        {
            agent.Warp(position);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().Warp(position.ToVector());

        }
    }
}
