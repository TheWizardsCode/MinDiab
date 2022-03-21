using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Character;
using WizardsCode.MinDiab.Combat;

namespace WizardsCode.MinDiab.Core
{
    public class Scheduler : MonoBehaviour
    {
        Queue<IAction> m_Queue = new Queue<IAction>();
        IAction currentAction;

        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.StopAction();
            }
            currentAction = action;
            currentAction.StartAction();
        }

        public void QueueAction(IAction action)
        {
            if (currentAction == null)
            {
                StartAction(action);
            }
            else
            {
                m_Queue.Enqueue(action);
            }
        }

        public void Update()
        {
            if (currentAction == null) return;

            currentAction.UpdateAction();
        }

        public void StopAction()
        {
            currentAction.StopAction();
            currentAction = null;
            if (m_Queue.Count > 0)
            {
                StartAction(m_Queue.Dequeue());
            }
        }

        internal bool IsActiveOrQueued(IAction action)
        {
            if (IsActive(action)) return true;

            return m_Queue.Contains(action);
        }

        internal bool IsActive(IAction action)
        {
            if (currentAction == action) return true;

            return false;
        }
    }
}
