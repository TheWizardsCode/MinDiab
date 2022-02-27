using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Configuration
{
    public static class AnimationParameters
    {
        const string m_StopActionTriggerName = "stopAction";
        static int? m_StopActionTriggerID;
        const string m_DefaultAttackTriggerName = "attack";
        static int? m_DefaultAttackTriggerID;
        const string m_DefaultDieTriggerName = "die";
        static int? m_DefaultDieTriggerID;

        public static int DefaultAttackTriggerID {
            get {
                if (m_DefaultAttackTriggerID == null) { 
                    m_DefaultAttackTriggerID = Animator.StringToHash(m_DefaultAttackTriggerName);
                }
                return (int)m_DefaultAttackTriggerID;
             }
        }

        public static int DefaultDieTriggerID
        {
            get
            {
                if (m_DefaultDieTriggerID == null)
                {
                    m_DefaultDieTriggerID = Animator.StringToHash(m_DefaultDieTriggerName);
                }
                return (int)m_DefaultDieTriggerID;
            }
        }
        public static int StopActionTriggerID
        {
            get
            {
                if (m_StopActionTriggerID == null)
                {
                    m_StopActionTriggerID = Animator.StringToHash(m_StopActionTriggerName);
                }
                return (int)m_StopActionTriggerID;
            }
        }

    }
}
