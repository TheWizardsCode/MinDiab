using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WizardsCode.MinDiab.Configuration;
using WizardsCode.MinDiab.Controller;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.Character
{
    [RequireComponent(typeof(Scheduler))]
    public class MoveController : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField, Tooltip("The maximum speed of mocement under normal circumstances, i.e. with no buffs or nerfs.")]
        float m_MaxSpeed = 6;

        private CharacterRoleController controller;
        private NavMeshAgent agent;
        private Vector3 m_Destination;
        private float m_SpeedMultiplier;

        public bool AtDestination {
            get
            {
                return !agent.hasPath || agent.remainingDistance < agent.stoppingDistance;
            }
        }

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            controller = GetComponent<CharacterRoleController>();
        }

        public void MoveTo(Vector3 pos, float speedMultiplier)
        {
            m_Destination = pos;
            m_SpeedMultiplier = speedMultiplier;
            controller.scheduler.StartAction(this);
        }

        public void StartAction()
        {
            agent.enabled = true;
            agent.speed = m_MaxSpeed * m_SpeedMultiplier;
            agent.SetDestination(m_Destination);
            agent.isStopped = false;
        }

        public void UpdateAction()
        {
            UpdateAnimator();

            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        controller.scheduler.StopAction();
                    }
                }
            }
        }

        public void StopAction()
        {
            agent.isStopped = agent.enabled;
            controller.animator.SetFloat(AnimationParameters.ForwardSpeed, 0);
        }

        private void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
            float speed = localVelocity.z;
            controller.animator.SetFloat(AnimationParameters.ForwardSpeed, speed);
        }

        internal void Warp(Vector3 position)
        {
            agent.Warp(position);
        }

        public object CaptureState()
        {
            MoveControllerSaveData data = new MoveControllerSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.rotation.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            MoveControllerSaveData data = (MoveControllerSaveData)state;
            Warp(data.position.ToVector());
            transform.rotation = Quaternion.Euler(data.rotation.ToVector());
        }

        [Serializable]
        struct MoveControllerSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }
    }
}
