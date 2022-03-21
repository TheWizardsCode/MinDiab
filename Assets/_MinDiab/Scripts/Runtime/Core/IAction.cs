using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WizardsCode.MinDiab.Core
{
    public interface IAction
    {
        void StartAction();
        void UpdateAction();
        void StopAction();
    }
}
