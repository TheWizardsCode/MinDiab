using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Configuration;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.Character
{
    public class HealthController : MonoBehaviour, ISaveable
    {
        [SerializeField, Tooltip("The current health of this character in hitpoints.")]
        float m_HealthPoints = 100;

        Animator animator;
        Scheduler scheduler;

        public bool IsDead
        {
            get
            {
                return m_HealthPoints <= 0;
            }
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            animator = GetComponent<Animator>();
            scheduler = GetComponent<Scheduler>();
        }

        public void TakeDamage(float damage)
        {
            if (!IsDead)
            {
                m_HealthPoints = Mathf.Max(m_HealthPoints - damage, 0);
                if (m_HealthPoints == 0)
                {
                    Die();
                }
            }
        }

        void Die()
        {
            animator.SetTrigger(AnimationParameters.DefaultDieTriggerID);
            scheduler.StopCurrentAction();
        }

        public object CaptureState()
        {
            return m_HealthPoints;
        }

        public void RestoreState(object state)
        {
            m_HealthPoints = (float)state;

            if (m_HealthPoints <= 0)
            {
                
                Die();
            }
        }
    }
}
