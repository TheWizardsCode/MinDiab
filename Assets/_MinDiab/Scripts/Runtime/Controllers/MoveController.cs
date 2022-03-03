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

        private void Update()
        {
            UpdateAnimator();
        }

        public void MoveTo(Vector3 pos, float speedMultiplier)
        {
            agent.enabled = true;
            agent.speed = m_MaxSpeed * speedMultiplier;

            controller.scheduler.StartAction(this);
            agent.SetDestination(pos);
            agent.isStopped = false;
        }

        public void StopAction()
        {
            if (agent.enabled)
            {
                agent.isStopped = true;
            }
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
            GetComponent<NavMeshAgent>().Warp(data.position.ToVector());
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
