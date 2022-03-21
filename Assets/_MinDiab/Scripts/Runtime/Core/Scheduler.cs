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

        /// <summary>
        /// Start a new action if possible. If the action can be taken
        /// then the current action is stopped and the new action is started.
        /// </summary>
        /// <param name="action">The action to be started.</param>
        /// <param name="isOverride">This new action will jump to the front of the queue
        /// of actions that have been requesteed if 'isOverride' is set to 
        /// true (the default). If set to false then the action will only be started
        /// if there is no curently active action and the queue is empty.</param>
        /// <returns>True if the action is started, otherwise false.</returns>
        public bool StartAction(IAction action, bool isOverride = true)
        {
            if (!isOverride && currentAction == action) return false;

            if (isOverride && currentAction != null)
            {
                StopAction(false);
            }

            if (currentAction == null)
            {
                currentAction = action;
                currentAction.StartAction();
                return true;
            } else
            {
                return false;
            }
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

        /// <summary>
        /// Stop the currently active action and, if appropriate
        /// start the next action in the queue.
        /// </summary>
        /// <param name="isOverride">If set to false (default) the next action in the queue will be started. 
        /// If true then the next action in the queue will not be started, this allows for a new action to be
        /// inserted at the head of the queue.</param>
        public void StopAction(bool isOverride = false)
        {
            currentAction.StopAction();
            currentAction = null;
            if (!isOverride && m_Queue.Count > 0)
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
