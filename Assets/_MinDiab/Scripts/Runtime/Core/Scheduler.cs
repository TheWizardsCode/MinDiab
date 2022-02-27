using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WizardsCode.MinDiab.Core
{
    public class Scheduler : MonoBehaviour
    {
        IAction currentAction;

        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.StopAction();
            }
            currentAction = action;
        }

        public void StopCurrentAction()
        {
            StartAction(null);
        }
    }
}
