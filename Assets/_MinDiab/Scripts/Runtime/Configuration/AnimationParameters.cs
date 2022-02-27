using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Configuration
{
    public static class AnimationParameters
    {

        const string FORWARD_SPEED = "forwardSpeed";
        static int? m_ForwardSpeed;

        const string STOP_ACTION = "stopAction";
        static int? m_StopAction;
        const string DEFAULT_ATTACK = "attack";
        static int? m_DefaultAttack;
        const string DEFAULT_DIE = "die";
        static int? m_DefaultDie;

        public static int ForwardSpeed
        {
            get
            {
                if (m_ForwardSpeed == null)
                {
                    m_ForwardSpeed = Animator.StringToHash(FORWARD_SPEED);
                }
                return (int)m_ForwardSpeed;
            }
        }

        public static int DefaultAttackTriggerID {
            get {
                if (m_DefaultAttack == null) { 
                    m_DefaultAttack = Animator.StringToHash(DEFAULT_ATTACK);
                }
                return (int)m_DefaultAttack;
             }
        }

        public static int DefaultDieTriggerID
        {
            get
            {
                if (m_DefaultDie == null)
                {
                    m_DefaultDie = Animator.StringToHash(DEFAULT_DIE);
                }
                return (int)m_DefaultDie;
            }
        }
        public static int StopActionTriggerID
        {
            get
            {
                if (m_StopAction == null)
                {
                    m_StopAction = Animator.StringToHash(STOP_ACTION);
                }
                return (int)m_StopAction;
            }
        }

    }
}
